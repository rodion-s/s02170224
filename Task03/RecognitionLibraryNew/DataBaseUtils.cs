using Microsoft.EntityFrameworkCore;


namespace RecognitionLibrary
{
    public class Result
    {
        public int ResultId { get; set; }
        public byte[] Hash { get; set; }
        public string Path { get; set; }
        public string Label { get; set; }
        public double Confidence { get; set; }
        public int CountReffered { get; set; }
        public virtual ImgDetail Detail { get; set; }
    };
    public class ImgDetail
    {
        public int ImgDetailId { get; set; }
        public byte[] RawImg { get; set; }
    };

    class MyResultContext : DbContext
    {
        public DbSet<Result> Results { get; set; }
        public DbSet<ImgDetail> ImgDetails { get; set; }

        /*public MyResultContext()
        {
            Database.EnsureDeleted();
            //Database.EnsureCreated();
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ImageRecognitionDB;Trusted_Connection=True;");
        //optionsBuilder.UseSqlite("Data Source=/../../../../RecognitionLibraryNew/images_new.db");

        //optionsBuilder.UseSqlite(@"Data Source=C:\foo_db\my_db_test.db");
        optionsBuilder.UseSqlite(@"Data Source=..\..\..\..\my_db_lol.db");
        public void Clear()
        {
            lock (this)
            {
                Results.RemoveRange(Results);
                ImgDetails.RemoveRange(ImgDetails);
                SaveChanges();
            }
        }
    }
}