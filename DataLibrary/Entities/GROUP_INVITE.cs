﻿namespace DataLibrary.Entities
{
    public class GROUP_INVITE
    {
        public int? ID_GROUP_INVITE { get; set; }
        public required int IDGROUP { get; set; }
        public int? IDUSER { get; set; }
        public string? EMAIL { get; set; }
        public required int IDAUTHOR { get; set; }
        public DateTime? DATE_ADD { get; set; }
        public int? PHONE_NUMBER { get; set; }
    }
}
