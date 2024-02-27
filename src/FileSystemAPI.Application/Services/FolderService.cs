using AutoMapper;
using FileSystemAPI.Application.Contracts.Persistence;
using FileSystemAPI.Application.Contracts.Services;
using FileSystemAPI.Application.Models;
using FileSystemAPI.Application.Models.Common;
using FileSystemAPI.Application.Requests.Folder;
using FileSystemAPI.Application.Responses.Folder;
using FileSystemAPI.Application.Validators.Folder;
using FileSystemAPI.Domain.Common;
using FileSystemAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public FolderService(IFolderRepository folderRepository, IFileRepository fileRepository, IMapper mapper) 
        {
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<CreateNewFolderResponse> CreateNewFolder(CreateNewFolderRequest createNewFolderRequest)
        {
            var createNewFolderResponse = new CreateNewFolderResponse();

            var validator = new CreateNewFolderValidator();
            var validationResult = await validator.ValidateAsync(createNewFolderRequest);

            if (validationResult.Errors.Count > 0)
            {
                createNewFolderResponse.Success = false;
                createNewFolderResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createNewFolderResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (createNewFolderResponse.Success)
            {
                try
                {
                    if (await _folderRepository.FolderExists(createNewFolderRequest.ParentFolderID) == false)
                    {
                        throw new Exception("Parent folder does not exists !");
                    }
                    if (await _folderRepository.FolderNameExistsInParent(createNewFolderRequest.FolderName, createNewFolderRequest.ParentFolderID))
                    {
                        throw new Exception("Folder with provided name already exists !");
                    }

                    var folder = _mapper.Map<Folder>(createNewFolderRequest);
                    folder.Active = true;

                    folder = await _folderRepository.AddAsync(folder);

                    createNewFolderResponse.Folder = _mapper.Map<FolderModel>(folder);
                }
                catch (Exception ex) 
                {
                    createNewFolderResponse.Success = false;
                    createNewFolderResponse.Message = ex.Message;
                    return createNewFolderResponse;
                }
            }

            return createNewFolderResponse;
        }

        public async Task<DeleteFolderResponse> DeleteFolder(DeleteFolderRequest deleteFolderRequest)
        {
            var deleteFolderResponse = new DeleteFolderResponse();

            var validator = new DeleteFolderValidator();
            var validationResult = await validator.ValidateAsync(deleteFolderRequest);

            if (validationResult.Errors.Count > 0)
            {
                deleteFolderResponse.Success = false;
                deleteFolderResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    deleteFolderResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (deleteFolderResponse.Success)
            {
                try
                {
                    Folder? folder = await _folderRepository.GetByIdAsync(deleteFolderRequest.FolderID)!;

                    if (folder is null || folder.Active == false)
                    {
                        throw new Exception("Folder does not exists !");
                    }

                    await _folderRepository.DeleteFolder(deleteFolderRequest.FolderID);
                }
                catch (Exception ex)
                {
                    deleteFolderResponse.Success = false;
                    deleteFolderResponse.Message = ex.Message;
                    return deleteFolderResponse;
                }
            }

            return deleteFolderResponse;
        }

        public async Task<ListDirectoryResponse> ListDirectory(ListDirectoryRequest listDirectoryRequest)
        {
            var listDirectoryResponse = new ListDirectoryResponse();

            var validator = new ListDirectoryValidator();
            var validationResult = await validator.ValidateAsync(listDirectoryRequest);

            if (validationResult.Errors.Count > 0)
            {
                listDirectoryResponse.Success = false;
                listDirectoryResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    listDirectoryResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (listDirectoryResponse.Success)
            {
                try
                {
                    Folder? folder = await _folderRepository.GetByIdAsync(listDirectoryRequest.FolderID)!;

                    if (folder is null || folder.Active == false)
                    {
                        throw new Exception("Folder does not exists !");
                    }

                    var folders = await _folderRepository.GetFoldersInDirectory(listDirectoryRequest.FolderID);
                    var files = await _fileRepository.GetFilesInDirectory(listDirectoryRequest.FolderID);

                    List<DirectoryListingItem> items =
                    [
                        .. _mapper.Map<List<DirectoryListingItem>>(folders),
                        .. _mapper.Map<List<DirectoryListingItem>>(files),
                    ];

                    listDirectoryResponse.DirectoryItems = items;
                }
                catch (Exception ex)
                {
                    listDirectoryResponse.Success = false;
                    listDirectoryResponse.Message = ex.Message;
                    return listDirectoryResponse;
                }
            }

            return listDirectoryResponse;
        }

        public async Task<RenameFolderResponse> RenameFolder(RenameFolderRequest renameFolderRequest)
        {
            var renameFolderResponse = new RenameFolderResponse();

            var validator = new RenameFolderValidator();
            var validationResult = await validator.ValidateAsync(renameFolderRequest);

            if (validationResult.Errors.Count > 0)
            {
                renameFolderResponse.Success = false;
                renameFolderResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    renameFolderResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (renameFolderResponse.Success)
            {
                try
                {
                    Folder? folder = await _folderRepository.GetByIdAsync(renameFolderRequest.FolderID)!;

                    if(folder is null || folder.Active == false)
                    {
                        throw new Exception("Folder does not exists !");
                    }
                    if (await _folderRepository.FolderNameExistsInParent(renameFolderRequest.NewFolderName, folder.ParentFolderId!.Value))
                    {
                        throw new Exception("Folder with provided name already exists !");
                    }

                    folder.FolderName = renameFolderRequest.NewFolderName;

                    await _folderRepository.UpdateAsync(folder);

                    renameFolderResponse.Folder = _mapper.Map<FolderModel>(folder);
                }
                catch (Exception ex)
                {
                    renameFolderResponse.Success = false;
                    renameFolderResponse.Message = ex.Message;
                    return renameFolderResponse;
                }
            }

            return renameFolderResponse;
        }
    }
}
