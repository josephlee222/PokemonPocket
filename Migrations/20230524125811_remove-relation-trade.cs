using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonPocket.Migrations
{
    /// <inheritdoc />
    public partial class removerelationtrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Trainers_TrainerId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_TrainerId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Trades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "Trades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trades_TrainerId",
                table: "Trades",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Trainers_TrainerId",
                table: "Trades",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");
        }
    }
}
