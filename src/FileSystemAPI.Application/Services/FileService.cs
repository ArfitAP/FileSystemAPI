using AutoMapper;
using FileSystemAPI.Application.Contracts.Infrastructure;
using FileSystemAPI.Application.Contracts.Persistence;
using FileSystemAPI.Application.Contracts.Services;
using FileSystemAPI.Application.Models;
using FileSystemAPI.Application.Requests.File;
using FileSystemAPI.Application.Responses.File;
using FileSystemAPI.Application.Validators.File;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAPI.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageHandler _storageHandler;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository, IConfiguration configuration, IMapper mapper, IFileStorageHandler storageHandler, IFolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
            _mapper = mapper;
            _storageHandler = storageHandler;
            _folderRepository = folderRepository;
        }

        public async Task<CreateNewFileResponse> CreateNewFile(CreateNewFileRequest createNewFileRequest)
        {
            var createNewFileResponse = new CreateNewFileResponse();

            var validator = new CreateNewFileValidator();
            var validationResult = await validator.ValidateAsync(createNewFileRequest);

            if (validationResult.Errors.Count > 0)
            {
                createNewFileResponse.Success = false;
                createNewFileResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createNewFileResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                Log.Error(string.Join(", ", validationResult.Errors));
            }

            if (createNewFileResponse.Success)
            {
                try
                {
                    if (await _folderRepository.FolderExists(createNewFileRequest.FolderID) == false)
                    {
                        throw new Exception("Parent folder does not exists !");
                    }              

                    if (await _fileRepository.FileNameExistsInParent(createNewFileRequest.FileName, createNewFileRequest.FolderID))
                    {
                        var file = await _fileRepository.GetFileByNameAndParent(createNewFileRequest.FileName, createNewFileRequest.FolderID);
                        file.StoredFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        long size = await _storageHandler.WriteFileToStorage(createNewFileRequest.bytesBase64, file.StoredFileName);

                        await _fileRepository.UpdateFile(file, size);

                        createNewFileResponse.File = _mapper.Map<FileModel>(file);
                    }
                    else
                    {
                        var file = _mapper.Map<Domain.Entities.File>(createNewFileRequest);
                        file.Active = true;
                        file.StoredFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        long size = await _storageHandler.WriteFileToStorage(createNewFileRequest.bytesBase64, file.StoredFileName);

                        file.Size = size;

                        file = await _fileRepository.AddFile(file);

                        var parent = await _folderRepository.GetByIdAsync(createNewFileRequest.FolderID);
                        file.Folder = parent!;
                        createNewFileResponse.File = _mapper.Map<FileModel>(file);
                    }                   
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    createNewFileResponse.Success = false;
                    createNewFileResponse.Message = ex.Message;
                    return createNewFileResponse;
                }
            }

            return createNewFileResponse;
        }

        public async Task<DeleteFileResponse> DeleteFile(DeleteFileRequest deleteFileRequest)
        {
            var deleteFileResponse = new DeleteFileResponse();

            var validator = new DeleteFileValidator();
            var validationResult = await validator.ValidateAsync(deleteFileRequest);

            if (validationResult.Errors.Count > 0)
            {
                deleteFileResponse.Success = false;
                deleteFileResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    deleteFileResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                Log.Error(string.Join(", ", validationResult.Errors));
            }

            if (deleteFileResponse.Success)
            {
                try
                {
                    Domain.Entities.File? file = await _fileRepository.GetByIdAsync(deleteFileRequest.FileID)!;

                    if (file is null || file.Active == false)
                    {
                        throw new Exception("File does not exists !");
                    }

                    await _fileRepository.DeleteFile(file);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    deleteFileResponse.Success = false;
                    deleteFileResponse.Message = ex.Message;
                    return deleteFileResponse;
                }
            }

            return deleteFileResponse;
        }

        public async Task<GetFileResponse> GetFile(GetFileRequest getFileRequest)
        {
            var getFileResponse = new GetFileResponse();

            var validator = new GetFileValidator();
            var validationResult = await validator.ValidateAsync(getFileRequest);

            if (validationResult.Errors.Count > 0)
            {
                getFileResponse.Success = false;
                getFileResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    getFileResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                Log.Error(string.Join(", ", validationResult.Errors));
            }

            if (getFileResponse.Success)
            {
                try
                {
                    Domain.Entities.File? file = await _fileRepository.GetByIdAsync(getFileRequest.FileID)!;

                    if (file is null || file.Active == false)
                    {
                        throw new Exception("File does not exists !");
                    }

                    byte[] bytes = await _storageHandler.ReadFileFromStorage(file.StoredFileName);

                    getFileResponse.Bytes = bytes;
                    getFileResponse.FileName = file.FileName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    getFileResponse.Success = false;
                    getFileResponse.Message = ex.Message;
                    return getFileResponse;
                }
            }

            return getFileResponse;
        }

        public async Task<RenameFileResponse> RenameFile(RenameFileRequest renameFileRequest)
        {
            var renameFileResponse = new RenameFileResponse();

            var validator = new RenameFileValidator();
            var validationResult = await validator.ValidateAsync(renameFileRequest);

            if (validationResult.Errors.Count > 0)
            {
                renameFileResponse.Success = false;
                renameFileResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    renameFileResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                Log.Error(string.Join(", ", validationResult.Errors));
            }

            if (renameFileResponse.Success)
            {
                try
                {
                    Domain.Entities.File? file = await _fileRepository.GetFileById(renameFileRequest.FileID)!;

                    if (file is null || file.Active == false)
                    {
                        throw new Exception("File does not exists !");
                    }
                    if (await _fileRepository.FileNameExistsInParent(renameFileRequest.NewFileName, file.FolderId))
                    {
                        throw new Exception("File with provided name already exists !");
                    }

                    file.FileName = renameFileRequest.NewFileName;

                    await _fileRepository.UpdateAsync(file);

                    renameFileResponse.File = _mapper.Map<FileModel>(file);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    renameFileResponse.Success = false;
                    renameFileResponse.Message = ex.Message;
                    return renameFileResponse;
                }
            }

            return renameFileResponse;
        }

        public async Task<SearchFileResponse> SearchFile(SearchFileRequest searchFileRequest)
        {
            var searchFileResponse = new SearchFileResponse();

            var validator = new SearchFileValidator();
            var validationResult = await validator.ValidateAsync(searchFileRequest);

            if (validationResult.Errors.Count > 0)
            {
                searchFileResponse.Success = false;
                searchFileResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    searchFileResponse.ValidationErrors.Add(error.ErrorMessage);
                }

                Log.Error(string.Join(", ", validationResult.Errors));
            }

            if (searchFileResponse.Success)
            {
                try
                {
                    if (searchFileRequest.AllFolders == false && await _folderRepository.FolderExists(searchFileRequest.FolderID!.Value) == false)
                    {
                        throw new Exception("Folder does not exists !");
                    }

                    long startFolderId = 1;
                    if(searchFileRequest.AllFolders == false) 
                    {
                        startFolderId = searchFileRequest.FolderID!.Value;
                    }

                    var result = await _fileRepository.SearchFile(startFolderId, searchFileRequest.SearchString);

                    searchFileResponse.SearchResult = _mapper.Map<List<FileModel>>(result);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);

                    searchFileResponse.Success = false;
                    searchFileResponse.Message = ex.Message;
                    return searchFileResponse;
                }
            }

            return searchFileResponse;
        }
    }
}
