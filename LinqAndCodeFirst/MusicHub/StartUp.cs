namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportAlbumsInfo(context, 9));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Include(a => a.Producer)
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .ToList()
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        s.Name,
                        s.Price,
                        WriterName = s.Writer.Name
                    }).OrderByDescending(s => s.Name)
                      .ThenBy(s => s.WriterName)
                      .ToList(),
                    AlbumPrice = a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var album in albums)
            {
                result.AppendLine($"-AlbumName: {album.AlbumName}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                result.AppendLine($"-ProducerName: {album.ProducerName}");
                result.AppendLine($"-Songs:");

                int songsCounter = 0;

                foreach (var song in album.Songs)
                {
                    songsCounter++;

                    result.AppendLine($"---#{songsCounter}");
                    result.AppendLine($"---SongName: {song.Name}");
                    result.AppendLine($"---Price: {song.Price:f2}");
                    result.AppendLine($"---Writer: {song.WriterName}");
                }

                result.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
