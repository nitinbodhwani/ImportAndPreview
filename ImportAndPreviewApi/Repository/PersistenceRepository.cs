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

			try
			{
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

			try
			{
					var groupedItems = (from rd in db.ReportData
										where //rd.ReportId == rptEntity.ReportId &&
										rd.EventDateTime.Day == day
										&& rd.EventDateTime.Month == month
										&& rd.EventDateTime.Year == year
										group rd by rd.CardNumber into rdGrouped
										orderby rdGrouped.Key ascending
										select new
										{
											groupingKey = rdGrouped.Key,
											groupedValue = rdGrouped.ToList()
										}
								).ToList();

					foreach (var item in groupedItems)
					{
						var firstInRecordOfTheDay = item.groupedValue.Where(x => x.ReaderId == 1).Count() > 0 ? item.groupedValue.Where(x => x.ReaderId == 1).Aggregate((i1, i2) => i1.EventDateTime < i2.EventDateTime ? i1 : i2) : null;
						var lastOutRecordOfTheDay = item.groupedValue.Where(x => x.ReaderId == 2).Count() > 0 ? item.groupedValue.Where(x => x.ReaderId == 2).Aggregate((i1, i2) => i1.EventDateTime > i2.EventDateTime ? i1 : i2) : null;

					rptAggregateList.Add(new ReportAggregate
					{
						EmployeeName = item.groupedValue[0].EmployeeName,
						EmployeeCode = item.groupedValue[0].EmployeeCode,
						EventDate = new DateTime(year: year, month: month, day: day),
						Location = item.groupedValue[0].Location,
						InTime = firstInRecordOfTheDay != null ? firstInRecordOfTheDay.EventDateTime.ToShortTimeString() : "",
						OutTime = lastOutRecordOfTheDay != null ? lastOutRecordOfTheDay.EventDateTime.ToShortTimeString() : "",
						TotalHours = firstInRecordOfTheDay != null && lastOutRecordOfTheDay != null ? Math.Round((lastOutRecordOfTheDay.EventDateTime - firstInRecordOfTheDay.EventDateTime).TotalHours, 2) : 0
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

			string interimFirstInTime;
			string interimLastOutTime;

			try
			{
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
									EmployeeName = filteredData[0].EmployeeName,
									EmployeeCode = filteredData[0].EmployeeCode,
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
		/// Method to get Aggregated report for all cards per given card, month and year
		/// </summary>
		/// <param name="month">Month</param>
		/// <param name="year">Year</param>
		/// <param name="filterValue">Filter Value</param>
		/// <returns>Aggregated report for all cards per given card, month and year</returns>
		public IList<EmployeeAttendance> GetReportAggregatedByMonth(int month, int year, string filterValue)
		{
			IList<EmployeeAttendance> userAttendanceList = new List<EmployeeAttendance>();
			string interimFirstInTime;
			string interimLastOutTime;

			try 
			{
				List<ReportDataEntity> filteredData = null;
				if (filterValue != string.Empty)
				{
					filteredData = (from rd in db.ReportData
									where
									(rd.EmployeeName.StartsWith(filterValue) || rd.EmployeeCode.StartsWith(filterValue))
									&& rd.EventDateTime.Month == month
									&& rd.EventDateTime.Year == year
									select rd).ToList();
				}
				else
				{
					filteredData = (from rd in db.ReportData
									where rd.EventDateTime.Month == month
									&& rd.EventDateTime.Year == year
									select rd
											).ToList();
				}

				if (filteredData != null && filteredData.Count > 0)
				{
					var groupedData = (from fd in filteredData
									   group fd by fd.CardNumber into fdGrouped
									   select new
									   {
										   groupingKey = fdGrouped.Key,
										   groupedValue = fdGrouped.ToList()
									   }).ToList();

					int numberOfDaysInMonth = DateTime.DaysInMonth(year, month);

					foreach (var groupedItem in groupedData)
					{
						double aggregatedHours = 0;
						int monthlyEarnedPoints = 0;

						IList<SwipeInfo> swipeInfoCollection = new List<SwipeInfo>();

						for (int processingDay = 1; processingDay <= numberOfDaysInMonth; processingDay++)
						{
							double totalHoursInADay = 0;

							interimFirstInTime = string.Empty;
							interimLastOutTime = string.Empty;

							var firstInRecordOfTheDay = (from gi in groupedItem.groupedValue
														 where gi.EventDateTime.Day == processingDay
														 && gi.EventDateTime.Month == month
														 && gi.EventDateTime.Year == year
														 && gi.ReaderId == 1
														 orderby gi.EventDateTime ascending
														 select gi
													).FirstOrDefault();

							var lastOutRecordOfTheDay = (from gi in groupedItem.groupedValue
														 where gi.EventDateTime.Day == processingDay
														 && gi.EventDateTime.Month == month
														 && gi.EventDateTime.Year == year
														 && gi.ReaderId == 2
														 orderby gi.EventDateTime descending
														 select gi
													).FirstOrDefault();

							if (firstInRecordOfTheDay != null)
							{
								interimFirstInTime = firstInRecordOfTheDay.EventDateTime.ToShortTimeString();
							}

							if (lastOutRecordOfTheDay != null)
							{
								interimLastOutTime = lastOutRecordOfTheDay.EventDateTime.ToShortTimeString();
							}

							totalHoursInADay = (firstInRecordOfTheDay != null && lastOutRecordOfTheDay != null) ? Math.Round((lastOutRecordOfTheDay.EventDateTime - firstInRecordOfTheDay.EventDateTime).TotalHours, 2) : 0;

							if(totalHoursInADay >= 6) {
								monthlyEarnedPoints++;
							}
							
							// Sum up total ours in a day to compute aggregate for a month
							aggregatedHours += totalHoursInADay;

							swipeInfoCollection.Add(new SwipeInfo()
							{
								EventDate =  new DateTime(year: year, month: month, day: processingDay),
								Location = firstInRecordOfTheDay != null ? firstInRecordOfTheDay.Location : (lastOutRecordOfTheDay != null ? lastOutRecordOfTheDay.Location : string.Empty),
								InTime = interimFirstInTime,
								OutTime = interimLastOutTime,
								TotalHours = totalHoursInADay
							});
						}

						userAttendanceList.Add(new EmployeeAttendance
						{
							EmployeeName = groupedItem.groupedValue[0].EmployeeName,
							EmployeeCode = groupedItem.groupedValue[0].EmployeeCode,
							AggregatedHours = Math.Round(aggregatedHours, 2),
							MEP = monthlyEarnedPoints,
							SwipeInfoCollection = swipeInfoCollection
						});
					}
				}
				return userAttendanceList;
			}
			catch(Exception ex) 
			{
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
							AlarmText = reportData.AlarmText,
							EmployeeName = ExtractEmployeeFirstName(reportData.CardName) + " " + ExtractEmployeeLastName(reportData.CardName),
							EmployeeCode = ExtractEmployeeCode(reportData.CardName)
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

		private string ExtractEmployeeFirstName(string cardName) {
			string firstName = string.Empty;

			int indexOfFirstDigit = cardName.IndexOfAny("0123456789".ToCharArray());

			firstName = cardName.Substring(0, indexOfFirstDigit - 1);
			return firstName;
		}

		private string ExtractEmployeeLastName(string cardName)
		{
			string lastName = string.Empty;

			int indexOfLastDigit = cardName.LastIndexOfAny("0123456789".ToCharArray());

			if (cardName.Length > indexOfLastDigit + 2)
			{
				lastName = cardName.Substring(indexOfLastDigit + 2);
			}
			return lastName;
		}

		private string ExtractEmployeeCode(string cardName)
		{
			string employeeCode = string.Empty;

			int indexOfFirstDigit = cardName.IndexOfAny("0123456789".ToCharArray());
			int indexOfLastDigit = cardName.LastIndexOfAny("0123456789".ToCharArray());

			employeeCode = cardName.Substring(indexOfFirstDigit, (indexOfLastDigit - indexOfFirstDigit) + 1);

			return employeeCode;
		}
	}
}