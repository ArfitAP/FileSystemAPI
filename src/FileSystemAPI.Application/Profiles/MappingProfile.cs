using AutoMapper;
using FileSystemAPI.Application.Models;
using FileSystemAPI.Application.Models.Common;
using FileSystemAPI.Application.Requests.File;
using FileSystemAPI.Application.Requests.Folder;
using FileSystemAPI.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateNewFolderRequest, Folder>().ForMember(f => f.Id, opt => opt.Ignore())
                                                       .ForMember(f => f.Size, opt => opt.Ignore())
                                                       .ForMember(f => f.Active, opt => opt.Ignore());

            CreateMap<Folder, FolderModel>().ForMember(f => f.Type, opt => opt.Ignore());

            CreateMap<CreateNewFileRequest, Domain.Entities.File>().ForMember(f => f.Id, opt => opt.Ignore())
                                                                   .ForMember(f => f.Size, opt => opt.Ignore())
                                                                   .ForMember(f => f.StoredFileName, opt => opt.Ignore())
                                                                   .ForMember(f => f.Active, opt => opt.Ignore());

            CreateMap<Domain.Entities.File, FileModel>().ForMember(f => f.Type, opt => opt.Ignore())
                                                        .ForMember(f => f.FullPath, opt => opt.MapFrom(f => f.Folder.FullPath));

            CreateMap<Folder, DirectoryListingItem>().ForMember(f => f.Type, opt => opt.MapFrom(f => (int)ItemType.Folder))
                                                     .ForMember(f => f.Name, opt => opt.MapFrom(f => f.FolderName))
                                                     .ForMember(f => f.FolderId, opt => opt.MapFrom(f => f.ParentFolderId));

            CreateMap<Domain.Entities.File, DirectoryListingItem>().ForMember(f => f.Type, opt => opt.MapFrom(f => (int)ItemType.File))
                                                                   .ForMember(f => f.Name, opt => opt.MapFrom(f => f.FileName))
                                                                   .ForMember(f => f.FolderId, opt => opt.MapFrom(f => f.FolderId));
        }
    }
}
