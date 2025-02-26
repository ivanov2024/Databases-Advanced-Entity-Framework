namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId.HasValue &&
                            a.ProducerId.Value == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer!.Name,
                    AlbumSongs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price.ToString("F2"),
                            SongWriterName = s.Writer.Name,
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(w => w.SongWriterName)
                        .ToArray(),
                    TotalPrice = a.Price,
                })
                .ToArray();

            albums = albums
                .OrderByDescending(a => a.TotalPrice)
                .ToArray();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}\n" +
                    $"-ReleaseDate: {album.AlbumReleaseDate}\n" +
                    $"-ProducerName: {album.ProducerName}\n" +
                    $"-Songs:");

                int counter = 1;
                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{counter}\n" +
                        $"---SongName: {song.SongName}\n" +
                        $"---Price: {song.SongPrice}\n" +
                        $"---Writer: {song.SongWriterName}");

                    counter++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            TimeSpan durationSpan = TimeSpan.FromSeconds(duration);

            var songs = context.Songs
                .Where(s => s.Duration > durationSpan)
                .Select(s => new
                {
                    SongName = s.Name,
                    SongPerformers = s.SongPerformers
                        .Select(sp => new
                        {
                            PerfomerFirstName = sp.Performer.FirstName,
                            PerformerLastName = sp.Performer.LastName,
                        }) 
                        .OrderBy(sp => sp.PerfomerFirstName)
                        .ThenBy(sp => sp.PerformerLastName)
                        .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = (s.Album != null) ?
                    (s.Album.Producer != null ?
                        s.Album.Producer.Name : null) : (null),
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(w => w.WriterName)
                .ToArray();

            int counter = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{counter}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.SongPerformers)
                {
                    sb.AppendLine($"---Performer: {performer.PerfomerFirstName} {performer.PerformerLastName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");

                counter++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
