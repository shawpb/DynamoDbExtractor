using Amazon.DynamoDBv2;
using System;

namespace DynamoDBExtractor
{
	public class DynamoDBClient
	{
		private static AmazonDynamoDBClient _instance;

		public static AmazonDynamoDBClient Instance
		{
			get
			{
				if(_instance == null)
				{
					createClient();
				}

				return _instance;
			}
		}

		private static bool createClient()
		{
			try { _instance = new AmazonDynamoDBClient(); }
			catch (Exception ex)
			{
				Console.WriteLine("FAILED to create a DynamoDB client; " + ex.Message);
				throw;
			}
			
			return true;
		}
	}
}