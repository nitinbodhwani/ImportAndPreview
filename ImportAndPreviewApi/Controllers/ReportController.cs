using System.Web.Http;
using ImportAndPreviewApi.Models;
using ImportAndPreviewApi.Interfaces;
using ImportAndPreviewApi.Repository;
using System;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace ImportAndPreviewApi.Controllers
{
	[RoutePrefix("api/report")]
	//[EnableCors(origins: "http://example.com", headers: "*", methods: "*")]
	public class ReportController : ApiController
	{
		private IBusinessRepository BusinessRepository;

		ReportController() {
			BusinessRepository = new BusinessRepository();
		}

		/// <summary>
		/// Api method to get report data by month and year
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>List of all report data matching given month and year</returns>

		// Get: api/report/3/2018
		[Route("")]
		[HttpGet]
		public IHttpActionResult GetReportDataByMonthAndYear(int month, int year)
		{
			IList<ReportData> rptDataList = null;
			try
			{
				rptDataList = BusinessRepository.GetReportDataByMonthAndYear(month, year);
				return Ok(rptDataList);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Api method to get report aggregate for given date
		/// </summary>
		/// <param name="day">Day of the month</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given date</returns>

		// Get: api/report/date/20/03/2018
		[Route("date")]
		[HttpGet]
		public IHttpActionResult GetReportAggregateByDate(int day, int month, int year)
		{
			IList<ReportAggregate> rptAggregateList = null;
			try
			{
				rptAggregateList = BusinessRepository.GetReportAggregateByDate(day, month, year);
				return Ok(rptAggregateList);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Api method to get report aggregate for given card and month
		/// </summary>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given card and month</returns>

		// Get: api/report/card/03/2018
		[Route("card")]
		[HttpGet]
		public IHttpActionResult GetReportAggregateByCardNumberAndMonth(int cardNumber, int month, int year)
		{
			IList<ReportAggregate> rptAggregateList = null;
			try
			{
				rptAggregateList = BusinessRepository.GetReportAggregateByCardNumberAndMonth(cardNumber, month, year);
				return Ok(rptAggregateList);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		/// <summary>
		/// Api method for saving report data
		/// </summary>
		/// <param name="report">Report content</param>
		/// <returns><c>true</c> if report data saved successfully else returns <c>false</c></returns>

		// POST: api/report
		[Route("submit")]
		[HttpPost]
		[AllowAnonymous]
		public IHttpActionResult PostReportData([FromBody] Report report) {
			try
			{
				bool isReportDataSaved = false;
				isReportDataSaved = BusinessRepository.SaveReportData(report);
				return Ok(isReportDataSaved);
			}
			catch(Exception ex) {
				return InternalServerError(ex);
			}
		}

		[HttpPost]
		[Route("test")]
		[AllowAnonymous]
		public IHttpActionResult TestPost([FromBody]int number)
		{
			return Ok(number);
		}
	}
}