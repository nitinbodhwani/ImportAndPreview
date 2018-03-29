using ImportAndPreviewApi.EntityModels;
using ImportAndPreviewApi.Interfaces;
using ImportAndPreviewApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImportAndPreviewApi.Repository
{
	public class PersistenceRepository : IPersistenceRepository
	{
		private AttendanceContext db = new AttendanceContext();

		/// <summary>
		/// Method to get report data by month and year
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Report data matching given month and year</returns>
		public IList<ReportData> GetReportDataByMonthAndYear(int month, int year) 
		{
			IList<ReportData> rptDataList = null;
			//IList<ReportDataEntity> rptDataEntityList = null;
			//ReportEntity rptEntity = null;

			try
			{
				//rptEntity = db.Report.FirstOrDefault(report => report.Month == month && report.Year == year);

				//if (rptEntity != null && rptEntity.ReportId > 0)
				//{
					rptDataList = (from rd in db.ReportData
								   where rd.EventDateTime.Month == month
										&& rd.EventDateTime.Year == year
								   select new ReportData()
								   {
									   ReportDataId = rd.ReportDataId,
									   ReportId = rd.ReportId,
									   Node = rd.Node,
									   Panel = rd.Panel,
									   Event = rd.Event,
									   EventDateTime = rd.EventDateTime,
									   CardNumber = rd.CardNumber,
									   CardName = rd.CardName,
									   Location = rd.Location,
									   ReaderId = rd.ReaderId,
									   In = rd.In,
									   Out = rd.Out,
									   Affiliation = rd.Affiliation,
									   AlarmText = rd.AlarmText
								   }).ToList<ReportData>();
				//}
				return rptDataList;
			}
			catch(Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Method to get report aggregate for given date
		/// </summary>
		/// <param name="day">Day of the month</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given date</returns>

		public IList<ReportAggregate> GetReportAggregateByDate(int day, int month, int year)
		{
			IList<ReportAggregate> rptAggregateList = new List<ReportAggregate>();

			//ReportEntity rptEntity = null;

			try
			{
				//rptEntity = db.Report.FirstOrDefault(report => report.Month == month && report.Year == year);

				//if (rptEntity != null)
				//{
					var groupedItems = (from rd in db.ReportData
										where //rd.ReportId == rptEntity.ReportId &&
										rd.EventDateTime.Day == day
										&& rd.EventDateTime.Month == month
										&& rd.EventDateTime.Year == year
										group rd by rd.CardNumber into rdGrouped
										select new
										{
											groupingKey = rdGrouped.Key,
											groupedValue = rdGrouped.ToList()
										}
								).ToList();

					foreach (var item in groupedItems)
					{
						var firstInRecordOfTheDay = item.groupedValue.Where(x => x.ReaderId == 1).Aggregate((i1, i2) => i1.EventDateTime < i2.EventDateTime ? i1 : i2);
						var lastOutRecordOfTheDay = item.groupedValue.Where(x => x.ReaderId == 2).Aggregate((i1, i2) => i1.EventDateTime > i2.EventDateTime ? i1 : i2);

						rptAggregateList.Add(new ReportAggregate
						{
							CardNumber = item.groupingKey,
							CardName = firstInRecordOfTheDay.CardName,
							EventDate = new DateTime(year: year, month: month, day: day),
							Location = firstInRecordOfTheDay.Location,
							InTime = firstInRecordOfTheDay.EventDateTime.ToShortTimeString(),
							OutTime = lastOutRecordOfTheDay.EventDateTime.ToShortTimeString(),
							TotalHours = Math.Round((lastOutRecordOfTheDay.EventDateTime - firstInRecordOfTheDay.EventDateTime).TotalHours, 2)
						});
					}
				//}

				return rptAggregateList;
			}
			catch(Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Method to get report aggregate for given card and month
		/// </summary>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <returns>Aggregated report for all cards per given card and month</returns>

		public IList<ReportAggregate> GetReportAggregateByCardNumberAndMonth(int cardNumber, int month, int year) {
			IList<ReportAggregate> rptAggregateList = new List<ReportAggregate>();

			//ReportEntity rptEntity = null;
			string interimFirstInTime;
			string interimLastOutTime;

			try
			{
				//rptEntity = db.Report.FirstOrDefault(report => report.Month == month && report.Year == year);

				//if (rptEntity != null) 
				//{
					var filteredData = (from rd in db.ReportData
										where //rd.ReportId == rptEntity.ReportId &&
										rd.CardNumber == cardNumber
										&& rd.EventDateTime.Month == month
										&& rd.EventDateTime.Year == year
										select rd
										).ToList();

					if(filteredData != null && filteredData.Count > 0)
					{
						int numberOfDaysInMonth = DateTime.DaysInMonth(year, month);
						for (int processingDay = 1; processingDay <= numberOfDaysInMonth; processingDay++)
						{
							interimFirstInTime = string.Empty;
							interimLastOutTime = string.Empty;

							var firstInRecordOfTheDay = (from rd in db.ReportData
														 where rd.EventDateTime.Day == processingDay
														 && rd.EventDateTime.Month == month
														 && rd.EventDateTime.Year == year
														 && rd.ReaderId == 1
														 orderby rd.EventDateTime ascending
														 select rd
													).FirstOrDefault();

							var lastOutRecordOfTheDay = (from rd in db.ReportData
														 where rd.EventDateTime.Day == processingDay
														 && rd.EventDateTime.Month == month
														 && rd.EventDateTime.Year == year
														 && rd.ReaderId == 2
														 orderby rd.EventDateTime descending
														 select rd
													).FirstOrDefault();

							if (firstInRecordOfTheDay != null)
							{
								interimFirstInTime = firstInRecordOfTheDay.EventDateTime.ToShortTimeString();
							}

							if (lastOutRecordOfTheDay != null)
							{
								interimLastOutTime = lastOutRecordOfTheDay.EventDateTime.ToShortTimeString();
							}

							rptAggregateList.Add(new ReportAggregate
								{
									CardNumber = filteredData[0].CardNumber,
									CardName = filteredData[0].CardName,
									EventDate = new DateTime(year: year, month: month, day: processingDay),
									Location = firstInRecordOfTheDay != null ? firstInRecordOfTheDay.Location : (lastOutRecordOfTheDay != null ? lastOutRecordOfTheDay.Location : string.Empty),//filteredData[0].Location,
									InTime = interimFirstInTime,
									OutTime = interimLastOutTime,
									TotalHours = (firstInRecordOfTheDay != null && lastOutRecordOfTheDay != null) ? Math.Round((lastOutRecordOfTheDay.EventDateTime - firstInRecordOfTheDay.EventDateTime).TotalHours, 2) : 0
								});
						}
					}
				//}
				return rptAggregateList;
			}
			catch(Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Method to Save Report Data
		/// </summary>
		/// <param name="report">Report</param>
		/// <returns><c>true</c> if Report data is successfully saved else returns <c>false</c></returns>
		public  bool SaveReportData(Report report)
		{
			bool isReportDataSaved = false;

			try
			{
				if (report != null && report.ReportDataList != null && report.ReportDataList.Count > 0)
				{
					ReportEntity rptEntity = new ReportEntity()
					{
						Name = report.Name,
						ImportDate = DateTime.Now
					};

					db.Report.Add(rptEntity);
					db.SaveChanges();

					foreach (var reportData in report.ReportDataList)
					{
						ReportDataEntity rptDataEntity = new ReportDataEntity()
						{
							ReportId = rptEntity.ReportId,
							Node = reportData.Node,
							Panel = reportData.Panel,
							Event = reportData.Event,
							EventDateTime = reportData.EventDateTime,
							CardNumber = reportData.CardNumber,
							CardName = reportData.CardName,
							Location = reportData.Location,
							ReaderId = reportData.ReaderId,
							In = reportData.In,
							Out = reportData.Out,
							Affiliation = reportData.Affiliation,
							AlarmText = reportData.AlarmText
						};

						db.ReportData.Add(rptDataEntity);
					}
					
					db.SaveChanges();
					isReportDataSaved = true;
				}
			}
			catch(Exception ex) {
				throw ex;
			}
			return isReportDataSaved;
		}
	}
}