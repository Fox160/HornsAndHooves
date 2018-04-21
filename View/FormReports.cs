﻿using Microsoft.Reporting.WinForms;
using Service.BindingModels;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace View
{
    public partial class FormReports : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormReports(IReportService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormReports_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.Value.ToShortDateString() +
                                            " по " + dateTimePickerTo.Value.ToShortDateString());
                reportViewer.LocalReport.SetParameters(parameter);

                var dataSource = service.GetOrdersList(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });
                ReportDataSource sourceOrders = new ReportDataSource("OrderDataSet", dataSource);
                reportViewer.LocalReport.DataSources.Add(sourceOrders);

                var dataRequestSource = service.GetRequestsList(new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.Value,
                    DateTo = dateTimePickerTo.Value
                });

                ReportDataSource sourceRequests = new ReportDataSource("RequestDataSet", dataRequestSource);
                reportViewer.LocalReport.DataSources.Add(sourceRequests);

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    service.SaveReportToFile(new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.Value,
                        DateTo = dateTimePickerTo.Value
                    });
                    MessageBox.Show("Отчет сохранен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
