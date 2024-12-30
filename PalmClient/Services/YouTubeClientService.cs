using System.Net;
using System.Linq;
using System.Text.Json;
using VideoLibrary;
using Newtonsoft.Json.Linq;
using PalmClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Web;
using System.Net.Http.Headers;
using System;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace PalmClient.Services
{
    public interface IYouTubeClientService
    {
        IAsyncEnumerable<int> DownloadYoutubeAudioAsync(string url, string FileName);
        Task<List<YoutubeVideoInfo>?> SearchVideoByName(string query);
        string GetSafeFileName(string name, char replace = ' ');
    }

    public class YouTubeClientService : IYouTubeClientService
    {
        public const string YouTubeSearchUrl = "https://www.youtube.com/results?search_query=";
        public const string YouTubeBase = "https://www.youtube.com";
        public async Task<List<YoutubeVideoInfo>?> SearchVideoByName(string query)
        {
            List<YoutubeVideoInfo> videos = new List<YoutubeVideoInfo>();
            try
            {
                using (var client = new HttpClient())
                {
                    var queryEncoded = query.Contains("//:") ? query : HttpUtility.UrlEncode(query);
                    string? response = await client.GetStringAsync(YouTubeSearchUrl + queryEncoded);
                    if (response != null)
                    {
                        string start = "var ytInitialData = ";
                        string end = "};";
                        var startIndex = response.IndexOf(start) + start.Length;
                        var endIndex = response.IndexOf(end, startIndex);

                        var InitialData = JObject.Parse(response.Substring(startIndex, endIndex + 1 - startIndex));

                        var results = InitialData?["contents"]?["twoColumnSearchResultsRenderer"]?["primaryContents"]?["sectionListRenderer"]?["contents"]?[0]?["itemSectionRenderer"]?["contents"];
                        if (results != null)
                        {
                            foreach (var item in results)
                            {
                                var video_info = item["videoRenderer"];
                                var title = video_info?["title"]?["runs"]?[0]?["text"];
                                var url = video_info?["navigationEndpoint"]?["commandMetadata"]?["webCommandMetadata"]?["url"];
                                var length = video_info?["lengthText"]?["simpleText"];
                                var views = video_info?["shortViewCountText"]?["simpleText"];
                                var channel = video_info?["ownerText"]?["runs"]?[0]?["text"];
                                var thumbnail = video_info?["thumbnail"]?["thumbnails"]?[0]?["url"];

                                if (title != null && url != null && length != null
                                    && channel != null && views != null)
                                {
                                    videos.Add(new YoutubeVideoInfo
                                    {
                                        Title = GetSafeFileName(title.ToString()),
                                        Url = YouTubeBase + url.ToString(),
                                        Duration = length.ToString(),
                                       // Channel = channel.ToString(),
                                       // Views = views.ToString(),
                                        Thumbnail = thumbnail.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return default;
            }

            return videos;
        }

        public async IAsyncEnumerable<int> DownloadYoutubeAudioAsync(string url, string fileName)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (streamInfo != null)
            {
                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                var totalBytes = streamInfo.Size.Bytes;
                var buffer = new byte[8192];
                long totalBytesRead = 0;

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    int bytesRead;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;
                        double percent = (double)totalBytesRead / totalBytes * 100;
                        yield return (int)percent;
                    }
                }
            }
        }


        public string GetSafeFileName(string name, char replace = ' ')
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        }
    }
}