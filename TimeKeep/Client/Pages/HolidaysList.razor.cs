using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using TimeKeep.Client.Services;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Pages
{
    [Authorize]
    public partial class HolidaysList
    {
        [Parameter]
        public int Year { get; set; }
        [Parameter]
        public List<Holiday> Holidays { get; set; }

        [Inject]
        private HolidaysService HolidaysService { get; set; }

        private SfGrid<Holiday> HolidayGrid;
        private readonly List<string> Toolbar = new() { "Print", "ExcelExport", "PdfExport" };

        protected override async Task OnInitializedAsync()
        {
            Holidays = await HolidaysService.GetHolidaysByYear(Year);
        }

        public async Task ToolbarClickHandler(ClickEventArgs args)
        {
            var exportColumns = new List<GridColumn>
            {
                #pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                new GridColumn() { Field = "Date", HeaderText = "Date", Type = ColumnType.Date, Format = "d", TextAlign = TextAlign.Right },
                new GridColumn() { Field = "Name", HeaderText = "Name", TextAlign = TextAlign.Left }
                #pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };

            if (args.Item.Id.Contains("pdfexport"))  //Id is combination of Grid's ID and itemname
            {
                PdfExportProperties ExportProperties = new()
                {
                    FileName = $"holidays-{DateTime.Today.Year}.pdf",
                    Columns = exportColumns
                };
                await HolidayGrid.PdfExport(ExportProperties);
            }

            if (args.Item.Id.Contains("excelexport")) //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExportProperties = new()
                {
                    FileName = $"holidays-{DateTime.Today.Year}.xlsx",
                    Columns = exportColumns
                };
                await HolidayGrid.ExcelExport(ExportProperties);
            }
        }
    }
}
