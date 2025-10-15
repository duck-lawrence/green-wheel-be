using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.CitizenIdentity.Response;
using Application.Dtos.Common.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Dtos.DriverLicense.Response;
using Application.Dtos.User.Request;
using Application.Dtos.User.Respone;
using Application.Helpers;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICitizenIdentityRepository _citizenIdentityRepository;
        private readonly IDriverLicenseRepository _driverLicenseRepository;

        public UserService(IUserRepository repository,
             IMapper mapper,
             ICitizenIdentityRepository citizenIdentityRepository,
             IDriverLicenseRepository driverLicenseRepository
            )
        {
            _userRepository = repository;
            _mapper = mapper;
            _citizenIdentityRepository = citizenIdentityRepository;
            _driverLicenseRepository = driverLicenseRepository;
        }



        public async Task<Guid> CreateAsync(CreateUserReq req)
        {
            if (await _userRepository.GetByPhoneAsync(req.Phone) != null)
            {
                throw new ConflictDuplicateException(Message.UserMessage.PhoneAlreadyExist);
            }
            var user = _mapper.Map<User>(req);
            user.Id = Guid.NewGuid();
            await _userRepository.AddAsync(user);
            return user.Id;
        }

        public async Task<IEnumerable<UserProfileViewRes>> GetAllAsync(
            string? phone,
            string? citizenIdNumber,
            string? driverLicenseNumber)
        {
            var users = await _userRepository.GetAllAsync(phone, citizenIdNumber, driverLicenseNumber);
            return _mapper.Map<IEnumerable<UserProfileViewRes>>(users);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<UserProfileViewRes> GetByPhoneAsync(string phone)
        {
            var user = await _userRepository.GetByPhoneAsync(phone);
            if (user == null)
            {
                throw new NotFoundException(Message.UserMessage.UserNotFound);
            }
            var userViewRes = _mapper.Map<UserProfileViewRes>(user);
            return userViewRes;
        }

        public async Task<UserProfileViewRes> GetByCitizenIdentityAsync(string idNumber)
        {
            var citizenIdentity = await _citizenIdentityRepository.GetByIdNumberAsync(idNumber);
            if (citizenIdentity == null)
            {
                throw new NotFoundException(Message.UserMessage.CitizenIdentityNotFound);
            }
            var userView = _mapper.Map<UserProfileViewRes>(citizenIdentity.User);
            return userView;
        }

        public async Task<UserProfileViewRes> GetByDriverLicenseAsync(string number)
        {
            var driverLicense = await _driverLicenseRepository.GetByLicenseNumber(number);
            if (driverLicense == null)
            {
                throw new NotFoundException(Message.UserMessage.CitizenIdentityNotFound);
            }
            var userView = _mapper.Map<UserProfileViewRes>(driverLicense.User);
            return userView;
        }


    }
}