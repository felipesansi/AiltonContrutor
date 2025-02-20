
using AiltonConstrutor.Models;
using AiltonConstrutor.Repositorio.Interfaces;
using CasaFacilEPS.Context;
using Microsoft.EntityFrameworkCore;

namespace AiltonConstrutor.Repositorio
{
    public class VideoRepositorio : IVideo
    {
        private readonly AppDbContext _appDbContext;

        public VideoRepositorio(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Video> GetAllVideos()
        {
            return _appDbContext.Videos.Include(v => v.Imovel);
        }

        public IEnumerable<Video> GetVideosByImovelId(int idImovel)
        {
            return _appDbContext.Videos.Where(v => v.IdImovel == idImovel).Include(v => v.Imovel);
        }

        public Video? GetVideoById(int idVideo)
        {
            return _appDbContext.Videos.Include(v => v.Imovel).FirstOrDefault(v => v.IdVideo == idVideo);
        }

        public void AddVideo(Video video)
        {
            _appDbContext.Videos.Add(video);
            _appDbContext.SaveChanges();
        }

        public void UpdateVideo(Video video)
        {
            _appDbContext.Videos.Update(video);
            _appDbContext.SaveChanges();
        }

        public void DeleteVideo(int idVideo)
        {
            var video = _appDbContext.Videos.FirstOrDefault(v => v.IdVideo == idVideo);
            if (video != null)
            {
                _appDbContext.Videos.Remove(video);
                _appDbContext.SaveChanges();
            }
        }
    }
}
