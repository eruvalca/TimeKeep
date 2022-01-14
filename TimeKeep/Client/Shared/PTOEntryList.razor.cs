using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Models;

namespace TimeKeep.Client.Shared
{
    [Authorize]
    public partial class PTOEntryList
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public List<PTOEntry> PTOEntries { get; set; }

        private SfGrid<PTOEntry> DefaultGrid;
        private readonly List<string> Toolbar = new() { "Print", "ExcelExport", "PdfExport" };

        protected override void OnParametersSet()
        {
            PTOEntries = PTOEntries
                .Where(p => p.PTOType != PTOType.VacationCarriedOver)
                .Select(p =>
            new PTOEntry
            {
                PTOEntryId = p.PTOEntryId,
                PTODate = p.PTODate,
                PTOHours = (p.PTOHours * -1),
                PTOType = p.PTOType,
                CreateDate = p.CreateDate,
                ModifyDate = p.ModifyDate,
                TimeKeepUserId = p.TimeKeepUserId
            }).ToList();
        }

        public async Task ToolbarClickHandler(ClickEventArgs args)
        {
            var exportColumns = new List<GridColumn>
            {
                #pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                new GridColumn() { Field = "PTODate", HeaderText = "Date", Type = ColumnType.Date, Format = "d", TextAlign = TextAlign.Right },
                new GridColumn() { Field = "PTOType", HeaderText = "Type", TextAlign = TextAlign.Left },
                new GridColumn() { Field = "PTOHours", HeaderText = "Hours", Type = ColumnType.Number, Format = "N0", TextAlign = TextAlign.Right }
                #pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };

            if (args.Item.Id.Contains("pdfexport"))  //Id is combination of Grid's ID and itemname
            {
                PdfExportProperties ExportProperties = new()
                {
                    FileName = $"pto-entries-{DateTime.Today.Year}.pdf",
                    Columns = exportColumns
                };
                await DefaultGrid.PdfExport(ExportProperties);
            }

            if (args.Item.Id.Contains("excelexport")) //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExportProperties = new()
                {
                    FileName = $"pto-entries-{DateTime.Today.Year}.xlsx",
                    Columns = exportColumns
                };
                await DefaultGrid.ExcelExport(ExportProperties);
            }
        }
    }
}
