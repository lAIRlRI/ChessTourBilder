﻿using System.Data;

namespace ChessTourBuilderApp.Data.ChessClasses
{
    internal class MovePozition
    {
        public static readonly Func<IDataReader, MovePozition> mapper = r => new MovePozition()
        {
            Move = r["Move"].ToString(),
            Pozition = r["Pozition"].ToString()
        };

        public string Move { get; set; }
        public string Pozition { get; set; }

        public MovePozition(string move, string pozition)
        {
            Move = move;
            Pozition = pozition;
        }

        public MovePozition()
        {

        }
    }
}