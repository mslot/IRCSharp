﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace Migrations.Statistics
{
	[Migration(1)]
	class InitDatabase : Migration
	{
		public override void Down()
		{
			Create.Table("User")
				.WithColumn("nick").AsString()
				.WithColumn("id").AsInt32()
				.PrimaryKey()
				.Unique()
				.Indexed()
				.Identity();

			Create.Table("Channel")
				.WithColumn("name").AsString()
				.WithColumn("network").AsString()
				.WithColumn("userId").AsInt32().NotNullable()
				.WithColumn("id").AsInt32()
				.PrimaryKey()
				.Unique()
				.Indexed()
				.Identity();

			Create.Table("Query")
				.WithColumn("rawQuery").AsString()
				.WithColumn("channelId").AsInt32().NotNullable()
				.WithColumn("id").AsInt32()
				.PrimaryKey()
				.Unique()
				.Indexed()
				.Identity();

			Create.ForeignKey("fk_channel_user").FromTable("Channel").ForeignColumn("userId").ToTable("User").PrimaryColumn("id");
			Create.ForeignKey("fk_query_channel").FromTable("Query").ForeignColumn("channelId").ToTable("Channel").PrimaryColumn("id");
		}

		public override void Up()
		{
			Delete.Table("Query");
			Delete.Table("Channel");
			Delete.Table("User");
		}
	}
}
