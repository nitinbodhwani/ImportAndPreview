using ImportAndPreviewApi.Models;
using System.Collections.Generic;

namespace ImportAndPreviewApi.Interfaces
{
	interface IBusinessRepository
	{
		/// <summary>
		/// Method to Save Report Data
		/// </summary>
		/// <param name="report">Report</param>
		/// <returns><c>true</c> if Report data is successfully saved else returns <c>false</c></returns>
		bool SaveReportData(Report report);

		/// <summary>
		/// Method to get report data by month and year
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Report data matching given month and year</returns>
		IList<ReportData> GetReportDataByMonthAndYear(int month, int year);

		/// <summary>
		/// Method to get report aggregate for given date
		/// </summary>
		/// <param name="day">Day of the month</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given date</returns>

		IList<ReportAggregate> GetReportAggregateByDate(int day, int month, int year);

		/// <summary>
		/// Method to get report aggregate for given card and month
		/// </summary>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given card and month</returns>

		IList<ReportAggregate> GetReportAggregateByCardNumberAndMonth(int cardNumber, int month, int year);

		/// <summary>
		/// Method to get Aggregated report for all cards per given card, month and year
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <param name="filterValue">Filter Value</param>
		/// <returns>Aggregated report for all cards per given card, month and year</returns>
		IList<EmployeeAttendance> GetReportAggregatedByMonth(int month, int year, string filterValue);
	}
}
