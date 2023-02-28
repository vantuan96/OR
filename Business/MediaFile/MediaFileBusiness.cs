using System;
using System.Linq;
using BMS.Contract.MediaFile;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.MediaFile
{
    public interface IMediaFileBusiness
    {
        PagedList<MediaFileContract> GetListMediaFile(string fileName, int type, DateTime? fromDate, DateTime? toDate, int page, int pageSize);

        MediaFileContract GetMediaFile(int fileId);

        CUDReturnMessage InsertUpdateMediaFile(MediaFileContract content);
    }

    public class MediaFileBusiness : BaseBusiness, IMediaFileBusiness
    {
        private Lazy<IMediaFileDataAccess> lazyMediaFileDataAccess;

        public MediaFileBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyMediaFileDataAccess = new Lazy<IMediaFileDataAccess>(() => new MediaFileDataAccess(appid, uid));
        }

        

        public PagedList<MediaFileContract> GetListMediaFile(string fileName, int type, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = lazyMediaFileDataAccess.Value.GetListMediaFile(fileName, type,  fromDate,  toDate);
            var result = new PagedList<MediaFileContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.FileId).Skip((page - 1) * pageSize).Take(pageSize).Select(mediaFile => new MediaFileContract
                {
                    FileId = mediaFile.FileId,
                    FileName = mediaFile.FileName,
                    FileDesc = mediaFile.FileDesc,
                    FileType = mediaFile.FileType,
                    FileUrl = mediaFile.FileUrl,
                    CreatedBy = mediaFile.CreatedBy,
                    CreatedDate = mediaFile.CreatedDate,
                    LastUpdatedBy = mediaFile.LastUpdatedBy,
                    LastUpdatedDate = mediaFile.LastUpdatedDate,
                    FileSize = mediaFile.FileSize
                }).ToList();
            }

            return result;
        }

        public MediaFileContract GetMediaFile(int fileId)
        {
            var mediaFile = lazyMediaFileDataAccess.Value.GetMediaFile(fileId);

            if (mediaFile != null)
            {
                return new MediaFileContract() {
                    FileId = mediaFile.FileId,
                    FileName = mediaFile.FileName,
                    FileDesc = mediaFile.FileDesc,
                    FileType = mediaFile.FileType,
                    FileUrl = mediaFile.FileUrl,
                    CreatedBy = mediaFile.CreatedBy,
                    CreatedDate = mediaFile.CreatedDate,
                    LastUpdatedBy = mediaFile.LastUpdatedBy,
                    LastUpdatedDate = mediaFile.LastUpdatedDate,
                    FileSize = mediaFile.FileSize
                };
            }

            return null;

            
        }

        public CUDReturnMessage InsertUpdateMediaFile(MediaFileContract content)
        {
            return lazyMediaFileDataAccess.Value.InsertUpdateMediaFile(content);
        }
        
    }
}