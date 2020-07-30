﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Succubus.Database.Context;

namespace Succubus.Database.Migrations
{
    [DbContext(typeof(SuccubusContext))]
    [Migration("20200730194023_SetUpdate_YabaiLevel")]
    partial class SetUpdate_YabaiLevel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6");

            modelBuilder.Entity("Succubus.Database.Models.Cosplayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Aliases")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Booth")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Instagram")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("TEXT");

                    b.Property<string>("Twitter")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cosplayers");
                });

            modelBuilder.Entity("Succubus.Database.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CosplayerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("SetId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CosplayerId");

                    b.HasIndex("SetId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Succubus.Database.Models.Server", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("ServerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Succubus.Database.Models.Set", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Aliases")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CosplayerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SetPreview")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<int>("YabaiLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CosplayerId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Succubus.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("Experience")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Succubus.Database.Models.Image", b =>
                {
                    b.HasOne("Succubus.Database.Models.Cosplayer", "Cosplayer")
                        .WithMany()
                        .HasForeignKey("CosplayerId");

                    b.HasOne("Succubus.Database.Models.Set", "Set")
                        .WithMany("Images")
                        .HasForeignKey("SetId");
                });

            modelBuilder.Entity("Succubus.Database.Models.Set", b =>
                {
                    b.HasOne("Succubus.Database.Models.Cosplayer", "Cosplayer")
                        .WithMany("Sets")
                        .HasForeignKey("CosplayerId");
                });
#pragma warning restore 612, 618
        }
    }
}