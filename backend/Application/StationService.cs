using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Station.Respone;
using Application.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;
        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<StationViewRes>> GetAllStation()
        {
            var stations = await _stationRepository.GetAllAsync();
            if(stations == null)
            {
                throw new NotFoundException(Message.StationMessage.StationNotFound);
            }
            return _mapper.Map<IEnumerable<StationViewRes>>(stations);
        }
    }
}
