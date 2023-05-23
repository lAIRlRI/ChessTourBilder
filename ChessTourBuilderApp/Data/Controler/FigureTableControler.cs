﻿using ChessTourBuilderApp.Data.ChessClasses;
using ChessTourBuilderApp.Data.Model;
using Newtonsoft.Json;

namespace ChessTourBuilderApp.Data.Controler
{
    internal class FigureTableControler
    {
        public static async Task<bool> CreateFigureMove( string table)
        {
            string messege = await Api.ApiControler.Get($"FigureTableControler/createFigureMove?table={table}");
            if (messege == "Nice") return true;
            return false;
        }

        public static async Task<bool> InsertFigure( string table, FigureScheme value)
        {
            string messege = await Api.ApiControler.Post($"FigureTableControler/InsertFigure?table={table}", value);
            if (messege == "-1") return true;
            return false;
        }

        public static async Task<List<FigureScheme>> GetAll(string table)
        {
            return JsonConvert.DeserializeObject<List<FigureScheme>>(await Api.ApiControler.Get($"FigureTableControler/getAll?table={table}"));
        }

        public static async Task<bool> UpdatePozition(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updatePozition?table={table}", view);
            if (messege == "-1") return true;
            return false;
        }

        public static async Task<bool> UpdateInGame(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateInGame?table={table}", view);
            if (messege == "-1") return true;
            return false;
        }

        public static async Task<bool> UpdateCastlingLong(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateCastlingLong?table={table}", view);
            if (messege == "-1") return true;
            return false;
        }

        public static async Task<bool> UpdateCastlingShort(string table, UpdateFigureModel view)
        {
            string messege = await Api.ApiControler.Put($"FigureTableControler/updateCastlingShort?table={table}", view);
            if (messege == "-1") return true;
            return false;
        }
    }
}