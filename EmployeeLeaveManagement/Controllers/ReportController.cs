using AutoMapper;
using EmployeeLeaveManagementBLL.Services.Implementation;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Enums;
using EmployeeLeaveManagementWeb.ViewModels.ReportVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IReportServices _reportServices;

     
        private readonly ILeaveRequestServices _leaveRequestServices;

        public ReportController(IReportServices reportServices, ILeaveRequestServices leaveRequestServices)
        {
            _reportServices = reportServices;
            _leaveRequestServices = leaveRequestServices;
        }

      

        public async Task<IActionResult> DepartmentReport(CancellationToken ct)
        {
            var departments = await _reportServices.GetDepartmentReportDataAsync(ct);

            var vm = departments.Select(d => new DepartmentReportVM
            {
                Name = d.Name,
                EmployeeCount = d.Employees.Count,
                Budget = d.Budget,
                TotalSalaries = d.Employees.Sum(e => e.Salary)
            }).ToList();

            return View(vm);
        }
        public async Task<IActionResult> DepartmentReportPdf(CancellationToken ct)
        {
            var departments = await _reportServices.GetDepartmentReportDataAsync(ct);

            var vm = departments.Select(d => new DepartmentReportVM
            {
                Name = d.Name,
                EmployeeCount = d.Employees.Count,
                Budget = d.Budget,
                TotalSalaries = d.Employees.Sum(e => e.Salary)
            }).ToList();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header()
                        .Text("Department Headcount & Budget Report")
                        .SemiBold().FontSize(18);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Department").SemiBold();
                            header.Cell().Text("Employees").SemiBold();
                            header.Cell().Text("Budget").SemiBold();
                            header.Cell().Text("Salaries").SemiBold();
                            header.Cell().Text("Remaining").SemiBold();
                        });

                        foreach (var d in vm)
                        {
                            table.Cell().Text(d.Name);
                            table.Cell().Text(d.EmployeeCount.ToString());
                            table.Cell().Text(d.Budget.ToString("C"));
                            table.Cell().Text(d.TotalSalaries.ToString("C"));
                            table.Cell().Text(d.RemainingBudget.ToString("C"));
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated on ");
                            x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", "DepartmentReport.pdf");
        }
        public async Task<IActionResult> LeaveSummaryReport(CancellationToken ct)
        {
            var employees = await _reportServices.GetLeaveSummaryDataAsync(ct);

            var vm = employees.Select(e => new LeaveSummaryReportVM
            {
                EmployeeName = e.FullName,
                DepartmentName = e.Department.Name,
                TotalRequests = e.LeaveRequests.Count,
                ApprovedRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Approved),
                RejectedRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Rejected),
                PendingRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Pending),
                TotalApprovedDays = e.LeaveRequests
                    .Where(r => r.Status == LeaveRequestStatus.Approved)
                    .Sum(r => (r.EndDate - r.StartDate).Days + 1)
            })
            .OrderByDescending(v => v.TotalApprovedDays)
            .ToList();

            return View(vm);
        }
        public async Task<IActionResult> LeaveSummaryReportPdf(CancellationToken ct)
        {
            var employees = await _reportServices.GetLeaveSummaryDataAsync(ct);

            var vm = employees.Select(e => new LeaveSummaryReportVM
            {
                EmployeeName = e.FullName,
                DepartmentName = e.Department.Name,
                TotalRequests = e.LeaveRequests.Count,
                ApprovedRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Approved),
                RejectedRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Rejected),
                PendingRequests = e.LeaveRequests.Count(r => r.Status == LeaveRequestStatus.Pending),
                TotalApprovedDays = e.LeaveRequests
                    .Where(r => r.Status == LeaveRequestStatus.Approved)
                    .Sum(r => (r.EndDate - r.StartDate).Days + 1)
            })
            .OrderByDescending(v => v.TotalApprovedDays)
            .ToList();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Text("Leave Summary Report").SemiBold().FontSize(18);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Employee").SemiBold();
                            header.Cell().Text("Department").SemiBold();
                            header.Cell().Text("Approved").SemiBold();
                            header.Cell().Text("Rejected").SemiBold();
                            header.Cell().Text("Days").SemiBold();
                        });

                        foreach (var v in vm)
                        {
                            table.Cell().Text(v.EmployeeName);
                            table.Cell().Text(v.DepartmentName);
                            table.Cell().Text(v.ApprovedRequests.ToString());
                            table.Cell().Text(v.RejectedRequests.ToString());
                            table.Cell().Text(v.TotalApprovedDays.ToString());
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", "LeaveSummaryReport.pdf");
        }
        public async Task<IActionResult> PendingApprovalsReport(CancellationToken ct)
        {
            var allRequests = await _leaveRequestServices.GetAllAsync(ct);

            var vm = allRequests
                .Where(r => r.Status == LeaveRequestStatus.Pending)
                .Select(r => new PendingApprovalReportVM
                {
                    EmployeeName = r.Employee.FullName,
                    DepartmentName = r.Employee.Department.Name,
                    LeaveTypeName = r.LeaveType.Name,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    DaysWaiting = (DateTime.Now.Date - r.StartDate.Date).Days
                })
                .OrderByDescending(v => v.DaysWaiting)
                .ToList();

            return View(vm);
        }
        public async Task<IActionResult> PendingApprovalsReportPdf(CancellationToken ct)
        {
            var allRequests = await _leaveRequestServices.GetAllAsync(ct);

            var vm = allRequests
                .Where(r => r.Status == LeaveRequestStatus.Pending)
                .Select(r => new PendingApprovalReportVM
                {
                    EmployeeName = r.Employee.FullName,
                    DepartmentName = r.Employee.Department.Name,
                    LeaveTypeName = r.LeaveType.Name,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    DaysWaiting = (DateTime.Now.Date - r.StartDate.Date).Days
                })
                .OrderByDescending(v => v.DaysWaiting)
                .ToList();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Text("Pending Approvals Report").SemiBold().FontSize(18);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Employee").SemiBold();
                            header.Cell().Text("Department").SemiBold();
                            header.Cell().Text("Leave Type").SemiBold();
                            header.Cell().Text("Days Waiting").SemiBold();
                        });

                        foreach (var v in vm)
                        {
                            table.Cell().Text(v.EmployeeName);
                            table.Cell().Text(v.DepartmentName);
                            table.Cell().Text(v.LeaveTypeName);
                            table.Cell().Text(v.DaysWaiting.ToString());
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", "PendingApprovalsReport.pdf");
        }
    }
}