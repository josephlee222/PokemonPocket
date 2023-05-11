﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonPocket;

#nullable disable

namespace PokemonPocket.Migrations
{
    [DbContext(typeof(PokemonDataContext))]
    [Migration("20230511055801_init_pokemon")]
    partial class init_pokemon
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PokemonPocket.Pokemon", b =>
                {
                    b.Property<int>("PokemonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.Property<string>("EvolveTo")
                        .HasColumnType("longtext");

                    b.Property<int>("Exp")
                        .HasColumnType("int");

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("NoToEvolve")
                        .HasColumnType("int");

                    b.Property<string>("Skill")
                        .HasColumnType("longtext");

                    b.HasKey("PokemonId");

                    b.ToTable("Pokemons");
                });
#pragma warning restore 612, 618
        }
    }
}
