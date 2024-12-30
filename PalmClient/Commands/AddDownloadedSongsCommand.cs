using PalmClient.Extensions;
using PalmClient.Models;
using PalmClient.Services;
using PalmClient.Stores;
using PalmData.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PalmClient.Commands
{
    public class AddDownloadedSongsCommand : AsyncCommandBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;
        private readonly ObservableCollection<MediaModel> _observableSongs;

        public AddDownloadedSongsCommand(IMusicPlayerService musicService, MediaStore mediaStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore, ObservableCollection<MediaModel> observableSongs)
        {
            _mediaStore = mediaStore;
            _musicService = musicService;
            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;
            _observableSongs = observableSongs;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                string appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string songsDirectory = Path.Combine(appDirectory, "downloads");

                if (Directory.Exists(songsDirectory))
                {
                    var audioFiles = Directory.GetFiles(songsDirectory, "*.*", SearchOption.AllDirectories)
                                              .Where(file => PathExtension.HasAudioVideoExtensions(file));

                    var newMediaEntities = audioFiles.Select(file => new MediaEntity
                    {
                        PlayerlistId = _playlistBrowserNavigationStore.BrowserPlaylistId,
                        FilePath = file,
                    }).ToList();

                    await _mediaStore.AddRange(newMediaEntities);

                    foreach (var mediaEntity in newMediaEntities)
                    {
                        if (!_observableSongs.Any(s => s.Path == mediaEntity.FilePath))
                        {
                            _observableSongs.Add(new MediaModel
                            {
                                Playing = false,
                                Id = mediaEntity.Id,
                                Title = Path.GetFileNameWithoutExtension(mediaEntity.FilePath),
                                Path = mediaEntity.FilePath,
                                Duration = AudioUtills.DurationParse(mediaEntity.FilePath),
                                Number = _observableSongs.Count + 1
                            });
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Directory does not exist: {songsDirectory}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddDownloadedSongsCommand: {ex.Message}");
            }
        }
    }
}
