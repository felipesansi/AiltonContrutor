using AiltonConstrutor.Models;

namespace AiltonConstrutor.Repositorio.Interfaces
{
    public interface IVideo
    {
        IEnumerable<Video> GetAllVideos();
        IEnumerable<Video> GetVideosByImovelId(int idImovel);
        Video? GetVideoById(int idVideo);
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int idVideo);
    }
}
