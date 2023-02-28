using System;
using BMS.Business.MediaFile;
using BMS.Contract.MediaFile;
using BMS.Contract.Shared;

namespace BMS.Caching.MediaFile
{
    public interface IMediaFileCaching
    {
        PagedList<MediaFileContract> GetListMediaFile(string fileName, int type, DateTime? fromDate, DateTime? toDate, int page, int pageSize);

        MediaFileContract GetMediaFile(int fileId);

        CUDReturnMessage InsertUpdateMediaFile(MediaFileContract content);
        
    }

    public class MediaFileCaching : BaseCaching, IMediaFileCaching
    {
        private Lazy<IMediaFileBusiness> lazyMediaFileBusiness;

        public MediaFileCaching(/*string appid, int uid*/)  
        {
            lazyMediaFileBusiness = new Lazy<IMediaFileBusiness>(() => new MediaFileBusiness(appid, uid));
        }

        public PagedList<MediaFileContract> GetListMediaFile(string fileName, int type, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            return lazyMediaFileBusiness.Value.GetListMediaFile(fileName, type, fromDate, toDate, page, pageSize);
        }

        public MediaFileContract GetMediaFile(int fileId)
        {
            return lazyMediaFileBusiness.Value.GetMediaFile(fileId);
        }

        public CUDReturnMessage InsertUpdateMediaFile(MediaFileContract content)
        {
            return lazyMediaFileBusiness.Value.InsertUpdateMediaFile(content);
        }

        
    }
}