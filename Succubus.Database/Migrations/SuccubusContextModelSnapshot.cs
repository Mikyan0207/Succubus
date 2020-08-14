﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Succubus.Database;

namespace Succubus.Database.Migrations
{
    [DbContext(typeof(SuccubusContext))]
    partial class SuccubusContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-preview.7.20365.15");

            modelBuilder.Entity("Succubus.Database.Models.Cosplayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Keywords")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cosplayers");
                });

            modelBuilder.Entity("Succubus.Database.Models.Guild", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Locale")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Prefix")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Succubus.Database.Models.Set", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CosplayerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Folder")
                        .HasColumnType("TEXT");

                    b.Property<string>("Keywords")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Prefix")
                        .HasColumnType("TEXT");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CosplayerId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Succubus.Database.Models.Social", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CosplayerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CosplayerId");

                    b.ToTable("Social");
                });

            modelBuilder.Entity("Succubus.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Succubus.Database.Models.Set", b =>
                {
                    b.HasOne("Succubus.Database.Models.Cosplayer", "Cosplayer")
                        .WithMany("Sets")
                        .HasForeignKey("CosplayerId");
                });

            modelBuilder.Entity("Succubus.Database.Models.Social", b =>
                {
                    b.HasOne("Succubus.Database.Models.Cosplayer", "Cosplayer")
                        .WithMany("Socials")
                        .HasForeignKey("CosplayerId");
                });
#pragma warning restore 612, 618
        }
    }
}
