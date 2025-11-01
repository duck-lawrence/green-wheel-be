﻿using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.RentalContract.Request;
using Application.Dtos.Statistic.Responses;
using Application.Dtos.Vehicle.Respone;
using Application.Helpers;
using Domain.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class StatisticService : IStatisticService
    {
        private readonly IUserService _userService;
        private readonly IVehicleService _vehicleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IVehicleModelService _vehicleModelService;

        public StatisticService(
            IUserService userService,
            IVehicleService vehicleService,
            IInvoiceService invoiceService,
            IVehicleModelService vehicleModelService)
        {
            _userService = userService;
            _vehicleService = vehicleService;
            _invoiceService = invoiceService;
            _vehicleModelService = vehicleModelService;
        }

        public async Task<CustomerAnonymusRes?> GetAnonymusCustomer([FromQuery] PaginationParams pagination)
        {
            var customer = await _userService.GetAllWithPaginationAsync(null, null, null, "Customer", pagination);
            if (customer == null || !customer.Items.Any())
                throw new NotFoundException(Message.StatisticMessage.NoCustomerData);

            int lastMonth = StatisticHelper.GetLastMonth();
            int previousYear = StatisticHelper.GetLastMonthYear();
            var customerThisMonth = customer.Items.Count(x => x.CreatedAt.Month == DateTimeOffset.UtcNow.Month && x.CreatedAt.Year == DateTimeOffset.UtcNow.Year && x.Email == null);
            var customerLastMonth = customer.Items.Count(x => x.CreatedAt.Month == lastMonth && x.CreatedAt.Year == previousYear && x.Email == null);

            if (customerThisMonth == 0 && customerLastMonth == 0)
                throw new BusinessException(Message.StatisticMessage.FailedToCalculateCustomerChange);

            decimal changeRate = 0;
            if (customerLastMonth > 0)
                changeRate = ((decimal)(customerThisMonth - customerLastMonth) / customerLastMonth) * 100;
            else if (customerThisMonth > 0)
                changeRate = 100;

            return new CustomerAnonymusRes
            {
                CustomerAnonymusInThisMonth = customerThisMonth,
                CustomerAnonymusInLastMonth = customerLastMonth,
                ChangeRate = Math.Round(changeRate, 2)
            };
        }

        public async Task<CustomerRes?> GetCustomer([FromQuery] PaginationParams pagination)
        {
            var customer = await _userService.GetAllWithPaginationAsync(null, null, null, "Customer", pagination);
            if (customer == null || !customer.Items.Any())
                throw new NotFoundException(Message.StatisticMessage.NoCustomerData);

            int lastMonth = StatisticHelper.GetLastMonth();
            int previousYear = StatisticHelper.GetLastMonthYear();
            var customerThisMonth = customer.Items.Count(x => x.CreatedAt.Month == DateTimeOffset.UtcNow.Month && x.CreatedAt.Year == DateTimeOffset.UtcNow.Year);
            var customerLastMonth = customer.Items.Count(x => x.CreatedAt.Month == lastMonth && x.CreatedAt.Year == previousYear);

            if (customerThisMonth == 0 && customerLastMonth == 0)
                throw new BusinessException(Message.StatisticMessage.FailedToCalculateCustomerChange);

            decimal changeRate = 0;
            if (customerLastMonth > 0)
                changeRate = ((decimal)(customerThisMonth - customerLastMonth) / customerLastMonth) * 100;
            else if (customerThisMonth > 0)
                changeRate = 100;

            return new CustomerRes
            {
                CustomerInLastMonth = customerLastMonth,
                CustomerInThisMonth = customerThisMonth,
                ChangeRate = Math.Round(changeRate, 2)
            };
        }

        public async Task<TotalRevenueRes?> GetTotalRevenue(Guid? stationId, [FromQuery] PaginationParams pagination)
        {
            var invoice = await _invoiceService.GetAllInvoicesAsync(pagination);
            if (invoice == null || !invoice.Items.Any())
                throw new NotFoundException(Message.StatisticMessage.NoInvoiceData);

            int lastMonth = StatisticHelper.GetLastMonth();
            int previousYear = StatisticHelper.GetLastMonthYear();
            decimal totalThisMonth = 0;
            decimal totalLastMonth = 0;

            var currentMonthInvoices = invoice.Items
                .Where(x => x.Contract != null &&
                            x.CreatedAt.Month == DateTimeOffset.UtcNow.Month &&
                            x.CreatedAt.Year == DateTimeOffset.UtcNow.Year &&
                            x.Contract.StationId == stationId);

            foreach (var item in currentMonthInvoices)
            {
                try
                {
                    totalThisMonth += InvoiceHelper.CalculateTotalAmount(item);
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

            var lastMonthInvoices = invoice.Items
                .Where(x => x.Contract != null &&
                            x.CreatedAt.Month == lastMonth &&
                            x.CreatedAt.Year == previousYear &&
                            x.Contract.StationId == stationId);

            foreach (var item in lastMonthInvoices)
            {
                try
                {
                    totalLastMonth += InvoiceHelper.CalculateTotalAmount(item);
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

            if (totalThisMonth == 0 && totalLastMonth == 0)
                throw new BusinessException(Message.StatisticMessage.FailedToCalculateRevenue);

            decimal changeRate = 0;
            if (totalLastMonth > 0)
                changeRate = ((totalThisMonth - totalLastMonth) / totalLastMonth) * 100;
            else if (totalThisMonth > 0)
                changeRate = 100;

            return new TotalRevenueRes
            {
                TotalRevenueThisMonth = Math.Round(totalThisMonth, 2),
                TotalRevenueLastMonth = Math.Round(totalLastMonth, 2),
                ChangeRate = Math.Round(changeRate, 2)
            };
        }

        public async Task<TotalStatisticRes?> GetTotalStatistic(Guid? stationId, [FromQuery] PaginationParams pagination)
        {
            var invoice = await _invoiceService.GetAllInvoicesAsync(pagination);
            if (invoice == null || !invoice.Items.Any())
                throw new NotFoundException(Message.StatisticMessage.NoInvoiceData);

            int lastMonth = StatisticHelper.GetLastMonth();
            int previousYear = StatisticHelper.GetLastMonthYear();

            var invoiceThisMonth = invoice.Items.Count(x =>
                x.Contract != null &&
                x.CreatedAt.Month == DateTimeOffset.UtcNow.Month &&
                x.CreatedAt.Year == DateTimeOffset.UtcNow.Year &&
                x.Contract.StationId == stationId);

            var invoiceLastMonth = invoice.Items.Count(x =>
                x.Contract != null &&
                x.CreatedAt.Month == lastMonth &&
                x.CreatedAt.Year == previousYear &&
                x.Contract.StationId == stationId);

            if (invoiceThisMonth == 0 && invoiceLastMonth == 0)
                throw new BusinessException(Message.StatisticMessage.FailedToCalculateInvoiceChange);

            decimal changeRate = 0;
            if (invoiceLastMonth > 0)
                changeRate = ((decimal)(invoiceThisMonth - invoiceLastMonth) / invoiceLastMonth) * 100;
            else if (invoiceThisMonth > 0)
                changeRate = 100;

            return new TotalStatisticRes
            {
                TotalStatisticThisMonth = invoiceThisMonth,
                TotalStatisticLastMonth = invoiceLastMonth,
                ChangeRate = Math.Round(changeRate, 2)
            };
        }

        

        public async Task<VehicleModelsStatisticRes?> GetVehicleModelTotal(Guid? stationId)
        {
            var vehicles = await _vehicleService.GetAllAsync(stationId, null);
            if (vehicles == null || !vehicles.Any())
                throw new NotFoundException(Message.StatisticMessage.NoVehicleData);

            var vehicleModels = await _vehicleModelService.GetAllAsync();
            if (vehicleModels == null || !vehicleModels.Any())
                throw new NotFoundException(Message.VehicleModelMessage.NotFound);

            var groupedByModel = vehicles
                .GroupBy(v => v.ModelId)
                .ToList();

            var items = new List<VehicleModelsForStatisticRes>();

            foreach (var group in groupedByModel)
            {
                var model = vehicleModels.FirstOrDefault(m => m.Id == group.Key);
                var modelName = model?.Name ?? "Unknown Model";

                var availableCount = group.Count(v => v.Status == (int)VehicleStatus.Available);
                var rentedCount = group.Count(v => v.Status == (int)VehicleStatus.Rented);
                var maintenanceCount = group.Count(v => v.Status == (int)VehicleStatus.Maintenance);

                items.Add(new VehicleModelsForStatisticRes(
                    group.Key,
                    modelName,
                    availableCount,
                    rentedCount,
                    maintenanceCount
                ));
            }

            return new VehicleModelsStatisticRes
            {
                VehicleModelsForStatisticRes = items.ToArray()
            };
        }

        public async Task<VehicleTotalRes?> GetVehicleTotal(Guid? stationId)
        {
            var vehicle = await _vehicleService.GetAllAsync(stationId, null);
            if (vehicle == null || !vehicle.Any())
                throw new NotFoundException(Message.StatisticMessage.NoVehicleData);

            var total = vehicle.Count();
            var items = new List<VehicleStatusCountItem>();

            foreach (VehicleStatus status in Enum.GetValues(typeof(VehicleStatus)))
            {
                var vehiclesByStatus = await _vehicleService.GetAllAsync(stationId, (int)status);
                items.Add(new VehicleStatusCountItem((int)status, vehiclesByStatus.Count()));
            }

            return new VehicleTotalRes
            {
                Total = total,
                Items = items
            };
        }
    }
}
