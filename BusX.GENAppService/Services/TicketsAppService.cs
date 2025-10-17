using AutoMapper;
using BusX.Data.Models;
using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Request;
using BusXAppServiceModels.Response;
using BusX.AppService.Base;
using BusX.AppService.Extensions;
using BusX.AppService.Interfaces;
using BusX.Data.Interfaces;
using BusX.Models;
using BusX.Models.Base;
using Microsoft.AspNetCore.Http;
using X.PagedList.Extensions;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using Sprache;
namespace BusX.AppService.Services
{
    public class TicketsAppService(IUnitOfWork _uow, IMapper _mapper, IHttpContextAccessor _contextAccessor, ILogger<JourneysAppService> logger) : BaseAppService(_uow, _mapper, _contextAccessor), ITicketsAppService
    {

        private readonly ILogger<JourneysAppService> _logger = logger;
        public ServiceResult<CreateOrEditResponse> CreateOrEdit(TicketDto request)
        {
            #region Log
            _logger.LogInformation("TicketsAppService servisinin CreateOrEdit metodu çağrıldı: {@Request}", request);
            #endregion

            try
            {
                var ticket = Mapper.Map<Ticket>(request);
                if (request.TicketID > 0)
                {
                    #region Log
                    _logger.LogInformation("Bilet güncellendi: {@Ticket}", ticket);
                    #endregion

                    UOW.TicketRepository.Update(ticket);
                }
                else
                {
                    #region Log
                    _logger.LogInformation("Yeni bilet oluşturuldu: {@Ticket}", ticket);
                    #endregion

                    UOW.TicketRepository.Create(ticket);
                }
                return ServiceResult<CreateOrEditResponse>.Success(new CreateOrEditResponse() { ID = ticket.TicketID });
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService CreateOrEdit metodunda hata oluştu: {@Request}", request);
                #endregion

                return ServiceResult<CreateOrEditResponse>.Error(ex.Message);
            }
        }
        public ServiceResult<bool> Delete(int ID)
        {
            #region Log
            _logger.LogInformation("TicketsAppService servisinin Delete metodu çağrıldı. Silinecek TicketID: {TicketID}", ID);
            #endregion

            try
            {
                UOW.TicketRepository.Delete(UOW.TicketRepository.Find(ID));

                #region Log
                _logger.LogInformation("Bilet başarıyla silindi: {@Ticket}", ID);
                #endregion

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService Delete metodunda hata oluştu. TicketID: {TicketID}", ID);
                #endregion

                return ServiceResult<bool>.Error(ex.Message);
            }
        }
        public ServiceResult<DetailResponse<TicketDto>> Get(GetDetailRequest request)
        {

            #region Log
            _logger.LogInformation("TicketsAppService servisinin Get metodu çağrıldı. İstenen TicketID: {TicketID}", request.ID);
            #endregion

            try { return ServiceResult<DetailResponse<TicketDto>>.Success(new DetailResponse<TicketDto>() { Detail = Mapper.Map<TicketDto>(UOW.TicketRepository.Find(request.ID)) }); }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService Get metodunda hata oluştu. TicketID: {TicketID}", request.ID);
                #endregion

                return ServiceResult<DetailResponse<TicketDto>>.Error(ex.Message);
            }
        }
        public ServiceResult<DetailResponse<TicketItem>> Info(GetDetailRequest request)
        {

            #region Log
            _logger.LogInformation("TicketsAppService servisinin Info metodu çağrıldı. İstenen TicketID: {TicketID}", request.ID);
            #endregion

            try
            {
                var ticket = UOW.TicketRepository.Where(x => x.TicketID == request.ID) ?? throw new Exception("not_found");
                var res = ticket.Select(x => new TicketItem()
                {
                    TicketID = x.TicketID,
                    JourneyId = x.JourneyId,
                    PassengerName = x.PassengerName,
                    Pnr = x.Pnr,
                    SeatNo = x.SeatNo,
                    IsMale = x.IsMale,
                    PassengerSurname = x.PassengerSurname,
                    TcNo = x.TcNo
                }).FirstOrDefault();

                #region Log
                _logger.LogInformation("Bilet detayları başarıyla getirildi: {@Ticket}", res);
                #endregion

                return ServiceResult<DetailResponse<TicketItem>>.Success(new DetailResponse<TicketItem>() { Detail = res });
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService Info metodunda hata oluştu. TicketID: {TicketID}", request.ID);
                #endregion

                return ServiceResult<DetailResponse<TicketItem>>.Error(ex.Message);
            }
        }
        public ServiceResult<SearchResponse<TicketListItem>> Search(TicketSearch request)
        {
            #region Log
            _logger.LogInformation("TicketsAppService servisinin Search metodu çağrıldı. Arama kriterleri: {@Request}", request);
            #endregion

            try
            {
                var companies = UOW.TicketRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(request.PassengerName), x => x.PassengerName.ToLower().Contains(request.PassengerName.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(request.PassengerSurname), x => x.PassengerName.ToLower().Contains(request.PassengerSurname.ToLower()))
                    .WhereIf(!string.IsNullOrWhiteSpace(request.Pnr), x => x.Pnr.ToLower().Contains(request.Pnr.ToLower()))
                    .WhereIf(request.SeatNo.HasValue, x => x.SeatNo == request.SeatNo.Value)
                    .WhereIf(request.JourneyId.HasValue, x => x.JourneyId == request.JourneyId.Value)
                    .WhereIf(request.IsMale.HasValue, x => x.IsMale == request.IsMale.Value)
                    .WhereIf(request.TcNo.HasValue, x => x.TcNo == request.TcNo.Value)


                    .Select(x => new TicketListItem()
                    {
                        TicketID = x.TicketID,
                        JourneyId = x.JourneyId,
                        PassengerName = x.PassengerName,
                        Pnr = x.Pnr,
                        SeatNo = x.SeatNo,
                        IsMale = x.IsMale,
                        PassengerSurname = x.PassengerSurname,
                        TcNo = x.TcNo
                    }).OrderBy(x => x.TicketID).ToPagedList();

                #region Log
                _logger.LogInformation("Ticket araması başarıyla tamamlandı. Toplam kayıt: {TotalItemCount}", companies.TotalItemCount);
                #endregion

                return ServiceResult<SearchResponse<TicketListItem>>.Success(new SearchResponse<TicketListItem>() { SearchResult = [.. companies], TotalItemCount = companies.TotalItemCount });
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService Search metodunda hata oluştu. Arama kriterleri: {@Request}", request);
                #endregion

                return ServiceResult<SearchResponse<TicketListItem>>.Error(ex.Message);
            }
        }

        public ServiceResult<bool> Checkout(CheckoutDto request)
        {
            #region Log
            _logger.LogInformation("TicketsAppService servisinin Checkout metodu çağrıldı. İstek verisi: {@Request}", request);
            #endregion

            try
            {
                if (request.CardNumber.StartsWith("0"))
                    return ServiceResult<bool>.Error("Kart numarası 0 ile başlayamaz.");

                if (request.TcNo.Any(tc => tc.StartsWith("0")))
                    return ServiceResult<bool>.Error("TC No 0 ile başlayamaz.");

                if (request.TcNo.Count != request.TcNo.Distinct().Count())
                    return ServiceResult<bool>.Error("Tüm TC No’lar birbirinden farklı olmalıdır.");

                if (request.TcNo.Count > 4)
                    return ServiceResult<bool>.Error("En fazla 4 tane bilet satın alabilirsiniz.");


                var existingTickets = UOW.TicketRepository
                    .Where(x => x.JourneyId == request.JourneyID)
                    .ToList(); // örnek: TicketListItem[]

                //her koltuk için kontrol yap
                foreach (var seat in request.SeatInfos)
                {
                    int seatNo = seat.SeatNumber;
                    bool isMale = seat.IsMale;
                    string passengerSurname = seat.PassengerSurname;

                    // yandaki koltuğu hesaplama
                    int adjacentSeatNo = seatNo % 2 == 0 ? seatNo - 1 : seatNo + 1;

                    // daha önce alınmış bileti kontrol ediyorum
                    var existingNeighbor = existingTickets.FirstOrDefault(t => t.SeatNo == adjacentSeatNo);
                    if (existingNeighbor != null)
                    {
                        if (existingNeighbor.IsMale != isMale &&
                            !existingNeighbor.PassengerSurname.Equals(passengerSurname, StringComparison.OrdinalIgnoreCase))
                        {
                            return ServiceResult<bool>.Error(
                                $"{seatNo} numaralı koltuğun yanındaki koltukta farklı cinsiyet ve farklı soyisim var. Bu kombinasyon yasak."
                            );
                        }
                    }

                    // aynı request içindeki yandaki koltuğu kontrol et
                    var neighborInRequest = request.SeatInfos.FirstOrDefault(s => s.SeatNumber == adjacentSeatNo);
                    if (neighborInRequest != null)
                    {
                        if (neighborInRequest.IsMale != isMale &&
                            !neighborInRequest.PassengerSurname.Equals(passengerSurname, StringComparison.OrdinalIgnoreCase))
                        {
                            return ServiceResult<bool>.Error(
                                $"{seatNo} numaralı koltuğun yanındaki koltukta farklı cinsiyet ve farklı soyisim var. Bu kombinasyon yasak."
                            );
                        }
                    }
                }

                #region Log
                _logger.LogInformation("Checkout doğrulaması başarıyla tamamlandı. Sefer ID: {JourneyID}, Koltuk Sayısı: {SeatCount}", request.JourneyID, request.SeatInfos.Count);
                #endregion

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                #region Log
                _logger.LogError(ex, "TicketsAppService Checkout metodunda hata oluştu. İstek verisi: {@Request}", request);
                #endregion

                return ServiceResult<bool>.Error(ex.Message);
            }
        }

        public ServiceResult<HealthCheckItem> CheckHealth()
        {
            #region Log
            _logger.LogInformation("TicketsAppService servisinin CheckHealth metodu çağrıldı.");
            #endregion

            var response = new HealthCheckItem();

            try
            {

                response.ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                response.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
                response.MachineName = Environment.MachineName;

                bool dbOk = false;
                try
                {
                    dbOk = UOW.TicketRepository.GetAll().Any();
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
                    _logger.LogInformation("HealthCheck sonucu: {Status} - {Message}. Environment: {Env}, Machine: {Machine}",
                        response.Status, response.Message, response.Environment, response.MachineName);
                    #endregion
                }
                else
                {
                    response.Status = "Degraded";
                    response.Message = "Health check warning — Service is running but database connection failed.";

                    #region Log
                    _logger.LogWarning("HealthCheck sonucu: {Status} - {Message}. Environment: {Env}, Machine: {Machine}",
                        response.Status, response.Message, response.Environment, response.MachineName);
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
                _logger.LogError(ex, "TicketsAppService nin CheckHealth metodunda hata oluştu. Durum: {Status}, Mesaj: {Message}, Environment: {Env}, Machine: {Machine}",
                    response.Status, response.Message, response.Environment, response.MachineName);
                #endregion

                return ServiceResult<HealthCheckItem>.Error(response, "error");
            }
        }

    }
}