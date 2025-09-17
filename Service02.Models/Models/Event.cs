using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Service02.Models.Models;

public partial class Event
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public byte[] IpAddress { get; set; } = null!;

    public DateTime Connection { get; set; }
}
