using AutoMapper;
using BusX.Data.Models;
using BusX.AppService.Base;
using BusX.AppService.Extensions;
using BusX.AppService.Interfaces;
using BusX.AppServiceModels.Request;
using BusX.AppServiceModels.Response;
using BusX.Data.Interfaces;
using BusX.Models;
using BusX.Models.Base;
using Microsoft.AspNetCore.Http;
using X.PagedList.Extensions;
using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Response;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Sprache;
using Microsoft.Extensions.Logging;
using Azure.Core;
namespace BusX.AppService.Services
{
    public class JourneysAppService(IUnitOfWork _uow, IMapper _mapper, IHttpContextAccessor _contextAccessor, IMemoryCache memoryCache, ILogger<JourneysAppService> logger) : BaseAppService(_uow, _mapper, _contextAccessor), IJourneysAppService
    {
        private readonly IMemoryCache _cache = memoryCache;
        private readonly ILogger<JourneysAppService> _logger = logger;
        public ServiceResult<CreateOrEditResponse> CreateOrEdit(JourneyDto request)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin CreateOrEdit metodu çağrıldı: {@Request}", request); 
            #endregion

            try
            {
                var journey = Mapper.Map<Journey>(request);
                if (request.JourneyID > 0)
                {
                    UOW.JourneyRepository.Update(journey);
                    #region Log
                    _logger.LogInformation("Journey güncellendi: {@Journey}", journey); 
                    #endregion
                }
                else 
                { 
                    UOW.JourneyRepository.Create(journey);
                    #region Log
                    _logger.LogInformation("Journey eklendi: {@Journey}", journey);
                    #endregion
                }
                return ServiceResult<CreateOrEditResponse>.Success(new CreateOrEditResponse() { ID = journey.JourneyID });
            }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "Journey CreateOrEdit servisinde hata oluştu: {@Journey}", request);
                #endregion
                return ServiceResult<CreateOrEditResponse>.Error(ex.Message); 
            }
        }
        public ServiceResult<bool> Delete(int ID)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin Delete metodu çağrıldı: {@RequestID}", ID);
            #endregion

            try
            {
                UOW.JourneyRepository.Delete(UOW.JourneyRepository.Find(ID));
                #region Log
                _logger.LogInformation("Journey silindi: {@Journey}", ID);
                #endregion
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "Journey silinirken hata oluştu: {@RequestID}", ID);
                #endregion
                return ServiceResult<bool>.Error(ex.Message); 
            }
        }
        public ServiceResult<DetailResponse<JourneyDto>> Get(GetDetailRequest request)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin Get metodu çağrıldı: {@Request}", request);
            #endregion
            try { return ServiceResult<DetailResponse<JourneyDto>>.Success(new DetailResponse<JourneyDto>() { Detail = Mapper.Map<JourneyDto>(UOW.JourneyRepository.Find(request.ID)) }); }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "JourneysAppService Get metodunda hata oluştu: {@Request}", request);
                #endregion
                return ServiceResult<DetailResponse<JourneyDto>>.Error(ex.Message); 
            }
        }
        public ServiceResult<DetailResponse<JourneyItem>> Info(GetDetailRequest request)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin Info metodu çağrıldı: {@Request}", request);
            #endregion
            try
            {
                var journey = UOW.JourneyRepository.Where(x => x.JourneyID == request.ID) ?? throw new Exception("not_found");
                var res = journey.Select(x => new JourneyItem()
                {
                    JourneyID = x.JourneyID,
                    BasePrice = x.BasePrice,
                    Date = x.Date,
                    Departure = x.Departure,
                    From = x.From,
                    To = x.To,
                    Provider = x.Provider,
                    IsAir = x.IsAir,
                    IsService = x.IsService,
                    IsTv = x.IsTv,
                    IsWifi = x.IsWifi,
                    TotalSeat = x.TotalSeat
                }).FirstOrDefault();
                #region Log
                _logger.LogInformation("Journey bilgileri başarıyla getirildi: {@Journey}", res);
                #endregion
                return ServiceResult<DetailResponse<JourneyItem>>.Success(new DetailResponse<JourneyItem>() { Detail = res });
            }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "JourneysAppService Info metodunda hata oluştu: {@Request}", request);
                #endregion
                return ServiceResult<DetailResponse<JourneyItem>>.Error(ex.Message); 
            }
        }
        public ServiceResult<SearchResponse<JourneyListItem>> Search(JourneySearch request)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin Search metodu çağrıldı: {@Request}", request);
            #endregion
            try
            {
                #region Cache Response
                string cacheKey = $"journeys_{request.From}_{request.To}_{request.Date?.ToString("yyyyMMdd")}";

                if (_cache.TryGetValue(cacheKey, out SearchResponse<JourneyListItem> cachedResult))
                {
                    return ServiceResult<SearchResponse<JourneyListItem>>.Success(cachedResult);
                } 
                #endregion


                var journeys = UOW.JourneyRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(request.From), x => x.From.ToLower().Contains(request.From.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(request.To), x => x.To.ToLower().Contains(request.To.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Provider), x => x.Provider.ToLower().Contains(request.Provider.ToLower()))
                    .WhereIf(request.BasePrice.HasValue, x => x.BasePrice == request.BasePrice.Value)
                    .WhereIf(request.Departure.HasValue, x => x.Departure == request.Departure.Value)
                    .WhereIf(request.Date.HasValue, x => x.Date == request.Date.Value)
                    .WhereIf(request.TotalSeat.HasValue, x => x.TotalSeat == request.TotalSeat.Value)
                    .Select(x => new JourneyListItem()
                    {
                        JourneyID = x.JourneyID,
                        BasePrice = x.BasePrice,
                        Date = x.Date,
                        Departure = x.Departure,
                        From = x.From,
                        To = x.To,
                        Provider = x.Provider,
                        IsAir = x.IsAir,
                        IsService = x.IsService,
                        IsTv = x.IsTv,
                        IsWifi = x.IsWifi,
                        TotalSeat = x.TotalSeat
                    }).OrderBy(x => x.JourneyID).ToPagedList(request.Page, (int)request.PageSize);

                #region Cache Set
                var result = new SearchResponse<JourneyListItem>()
                {
                    SearchResult = journeys.ToList(),
                    TotalItemCount = journeys.TotalItemCount
                };
                _cache.Set(cacheKey, result, TimeSpan.FromSeconds(120)); 
                #endregion


                #region Log
                _logger.LogInformation("Journey arama sonuçları başarıyla getirildi: {@ResultCount} sonuç, CacheKey: {@CacheKey}", result.SearchResult.Count, cacheKey);
                #endregion

                return ServiceResult<SearchResponse<JourneyListItem>>.Success(new SearchResponse<JourneyListItem>() { SearchResult = [.. journeys], TotalItemCount = journeys.TotalItemCount });
            }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "JourneysAppService Search metodunda hata oluştu: {@Request}", request);
                #endregion
                return ServiceResult<SearchResponse<JourneyListItem>>.Error(ex.Message); 
            }
        }

        public ServiceResult<SearchResponse<StationListItem>> GetAllStations()
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin GetAllStations metodu çağrıldı.");
            #endregion
            try
            {
                var journeys = UOW.StationRepository.GetAll()

                    .Select(x => new StationListItem()
                    {
                        StationID = x.StationID,
                        City = x.City,
                        Name = x.Name
                    }).OrderBy(x => x.StationID).ToList();
                #region Log
                _logger.LogInformation("Tüm istasyonlar başarıyla getirildi: {@StationCount} istasyon bulundu.", journeys.Count);
                #endregion
                return ServiceResult<SearchResponse<StationListItem>>.Success(new SearchResponse<StationListItem>() { SearchResult = [.. journeys] });
            }
            catch (Exception ex) { return ServiceResult<SearchResponse<StationListItem>>.Error(ex.Message); }
        }

        public ServiceResult<SearchResponse<JourneySeatPlanListItem>> GetSeatsPlan(int journeyId)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin GetSeatsPlan metodu çağrıldı: {@JourneyID}", journeyId);
            #endregion

            try
            {
                
                var tickets = UOW.TicketRepository.Where(x => x.JourneyId == journeyId).ToList();

                
                var fullSeats = tickets.Select(t => new SeatInfo
                {
                    SeatNumber = t.SeatNo,
                    IsMale = t.IsMale
                }).ToList();

               
                var journey = UOW.JourneyRepository.Find(journeyId);
                if (journey == null)
                    return ServiceResult<SearchResponse<JourneySeatPlanListItem>>.Error("Journey not found");

                int totalSeats = journey.TotalSeat;

               
                var emptySeats = new List<int>();
                for (int i = 1; i <= totalSeats; i++)
                {
                    if (!fullSeats.Any(f => f.SeatNumber == i))
                        emptySeats.Add(i);
                }

                
                var seatPlan = new JourneySeatPlanListItem
                {
                    FullSeats = fullSeats,
                    EmptySeats = emptySeats
                };

                #region Log
                _logger.LogInformation("JourneySeatPlan başarıyla oluşturuldu: {@JourneyID}, FullSeats: {@FullSeatsCount}, EmptySeats: {@EmptySeatsCount}",journeyId, fullSeats.Count, emptySeats.Count);
                #endregion

                return ServiceResult<SearchResponse<JourneySeatPlanListItem>>.Success(
                    new SearchResponse<JourneySeatPlanListItem>
                    {
                        SearchResult = new List<JourneySeatPlanListItem> { seatPlan },
                        TotalItemCount = 1
                    });
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "GetSeatsPlan metodunda hata oluştu: {@JourneyID}", journeyId);
                #endregion
                return ServiceResult<SearchResponse<JourneySeatPlanListItem>>.Error(ex.Message);
            }
        }

        public ServiceResult<HealthCheckItem> CheckHealth()
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin CheckHealth metodu çağrıldı.");
            #endregion

            var response = new HealthCheckItem();
            try
            {
                // Zaman bilgisi
                response.ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                response.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
                response.MachineName = Environment.MachineName;

                // Veritabanı bağlantısı testi
                bool dbOk = false;
                try
                {
                    dbOk = UOW.JourneyRepository.GetAll().Any();
                }
                catch
                {
                    dbOk = false;
                }

                response.DatabaseConnected = dbOk;

                if (dbOk)
                {
                    response.Status = "Healthy";
                    response.Message = "Health check OK — Database connection and service are running fine.";

                    #region Log
                    _logger.LogInformation("HealthCheck başarılı: {@Response}", response);
                    #endregion

                }
                else
                {
                    response.Status = "Degraded";
                    response.Message = "Health check warning — Service is running but database connection failed.";

                    #region Log
                    _logger.LogWarning("HealthCheck uyarı durumu: {@Response}", response);
                    #endregion
                }

                return ServiceResult<HealthCheckItem>.Success(response);
            }
            catch (Exception ex)
            {
                response.Status = "Unhealthy";
                response.Message = $"Health check failed: {ex.Message}";
                response.DatabaseConnected = false;
                response.ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                response.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
                response.MachineName = Environment.MachineName;

                #region Log
                _logger.LogError(ex, "HealthCheck metodunda hata oluştu: {@Response}", response);
                #endregion

                return ServiceResult<HealthCheckItem>.Error(response, "error");
            }
        }

        public ServiceResult<CreateOrEditResponse> CreateOrEditJourneySeat(InProcessJourneySeatDto request)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin CreateOrEditJourneySeat metodu çağrıldı: {@Request}", request);
            #endregion

            try
            {
                var journey = Mapper.Map<InProcessJourneySeat>(request);
                if (request.InProcessJourneySeatID > 0)
                {
                    UOW.InProcessJourneySeatRepository.Update(journey);
                }
                else
                {
                    bool seatExists = UOW.InProcessJourneySeatRepository
                        .Where(x => x.JourneyID == request.JourneyID && x.SeatNo == request.SeatNo)
                        .Any();

                    if (seatExists) throw new SeatAlreadyReservedException("Seat already reserved");

                    UOW.InProcessJourneySeatRepository.Create(journey);
                }

                #region Log
                _logger.LogInformation("Yeni InProcessJourneySeat eklendi: {@JourneySeat}", journey);
                #endregion

                return ServiceResult<CreateOrEditResponse>.Success(new CreateOrEditResponse() { ID = journey.InProcessJourneySeatID });
            }
            catch (SeatAlreadyReservedException ex)
            {
                #region Log
                _logger.LogError(ex, "Koltuk rezerve hatası: {@Request}", request);
                #endregion

                return ServiceResult<CreateOrEditResponse>.Error(ex.Message);
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "CreateOrEditJourneySeat metodunda hata oluştu: {@Request}", request);
                #endregion

                return ServiceResult<CreateOrEditResponse>.Error(ex.Message);
            }
        }

        public class SeatAlreadyReservedException : Exception
        {
            public SeatAlreadyReservedException(string message) : base(message) { }
        }


        public ServiceResult<bool> DeleteJourneySeat(int journeyID, int seatNo)
        {
            #region Log
            _logger.LogInformation("JourneysAppService servisinin DeleteJourneySeat metodu çağrıldı: {@JourneyID}, {@SeatNo}", journeyID, seatNo);
            #endregion

            try
            {
                UOW.InProcessJourneySeatRepository.Delete(UOW.InProcessJourneySeatRepository.Where(x=>x.JourneyID == journeyID && x.SeatNo == seatNo).FirstOrDefault());
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex) 
            {
                #region Log
                _logger.LogError(ex, "DeleteJourneySeat metodunda hata oluştu: JourneyID={@JourneyID}, SeatNo={@SeatNo}", journeyID, seatNo);
                #endregion

                return ServiceResult<bool>.Error(ex.Message); 
            }
        }
    }
}