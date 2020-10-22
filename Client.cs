using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDBExtractor
{
	class Client
	{
		private readonly string tableName = "cfthsignup-ClientTable-1493FOVNV0WXJ";

		public bool GetClients()
		{
			try
			{
				var response = DynamoDBClient.Instance.Scan(new ScanRequest(tableName));

				Console.WriteLine("Number of Clients: " + response.ScannedCount);

				string dateValue = DateTime.Now.ToShortDateString().Replace("/", "-");

				string filePath = @"C:\Users\pauls\OneDrive\CFTH\ClientExtracts\ClientExtract" + dateValue + ".csv";

				using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
				{
					string[] keys = { "Id","LastName","FirstName", "StreetAddress","AptNum","City","Zip",
						"Phone", "Email","Agency", "AgencyRepLastName","AgencyRepFirstName",
						"WhoDelivers", "AlternateFirstName", "AlternateLastName", "AlternatePhone", 
						"FamilySize","SSILetter", "WICCard", "CommunityReferral", "SsdLetter", "BenefitCard", 
						"Unemployment","MuniHousing","HeapLetter","CreatedTimestamp",};

					StringBuilder headerRow = new StringBuilder();

					foreach(var key in keys)
					{
						headerRow.Append(key).Append(",");
					}

					file.WriteLine(headerRow.ToString().Remove(headerRow.Length - 1, 1));

					//Get All the records

					foreach (Dictionary<string, AttributeValue> item in response.Items)
					{
						StringBuilder itemValues = new StringBuilder();

						// Process the result.
						foreach (var key in keys)
						{
							item.TryGetValue(key, out AttributeValue value);
							if (value != null)
							{

								switch (key) {
									case ("FamilySize"):
										{
											itemValues.Append(value.N);
											break;
										}
									case ("WICCard"):
									case ("CommunityReferral"):
									case ("SsdLetter"):
									case ("BenefitCard"):
									case ("Unemployment"):
									case ("SSILetter"):
									case ("MuniHousing"):
									case ("HeapLetter"):
										{
											itemValues.Append(value.BOOL);
											break;
										}
									default:
										{
											itemValues.Append("\"").Append(value.S).Append("\"");
											break;
										}
								}
							}
							itemValues.Append(",");
						}

						file.WriteLine(itemValues.ToString().Remove(itemValues.Length - 1, 1));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("     FAILED to get the movie, because: {0}.", ex.Message);
			}
			return (false);
		}
	}
}