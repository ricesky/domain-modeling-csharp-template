using HotelReservationSystem.Domain.Entity;
using HotelReservationSystem.Domain.ValueObject;

namespace DomainModeling.Tests;

[TestClass]
public class ReservationTests
{
    [TestMethod]
    public void RoomType_ShouldCreateValidRoomType()
    {
        var roomType = new RoomType("Deluxe");
        Assert.AreEqual("Deluxe", roomType.Type);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RoomType_ShouldThrowException_ForInvalidRoomType()
    {
        new RoomType("Presidential");
    }

    [TestMethod]
    public void GuestName_ShouldCreateValidGuestName()
    {
        var guestName = new GuestName("John Doe");
        Assert.AreEqual("John Doe", guestName.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GuestName_ShouldThrowException_ForEmptyGuestName()
    {
        new GuestName("");
    }

    [TestMethod]
    public void Room_ShouldCreateRoomWithCapacity()
    {
        var room = new Room(new RoomType("Standard"), 5);
        Assert.IsTrue(room.IsAvailable());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Room_ShouldThrowException_ForZeroCapacity()
    {
        new Room(new RoomType("Deluxe"), 0);
    }

    [TestMethod]
    public void Room_ShouldReserveAndReleaseCorrectly()
    {
        var room = new Room(new RoomType("Deluxe"), 2);
        room.Reserve();
        room.Reserve();
        Assert.IsFalse(room.IsAvailable());
        room.Release();
        Assert.IsTrue(room.IsAvailable());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Room_ShouldThrowException_WhenNoRoomsAvailable()
    {
        var room = new Room(new RoomType("Suite"), 1);
        room.Reserve();
        room.Reserve();
    }

    [TestMethod]
    public void Reservation_ShouldCreateValidReservation()
    {
        var reservation = new Reservation(new RoomType("Deluxe"), new GuestName("Jane Doe"), DateTime.Now.AddDays(2), DateTime.Now.AddDays(7));
        Assert.AreEqual("Deluxe", reservation.RoomType.Type);
        Assert.AreEqual("Jane Doe", reservation.GuestName.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Reservation_ShouldThrowException_ForPastCheckInDate()
    {
        new Reservation(new RoomType("Standard"), new GuestName("John Doe"), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Reservation_ShouldThrowException_ForInvalidCheckOutDate()
    {
        new Reservation(new RoomType("Standard"), new GuestName("John Doe"), DateTime.Now.AddDays(1), DateTime.Now.AddDays(1));
    }

    [TestMethod]
    public void Reservation_ShouldModifyDatesCorrectly()
    {
        var reservation = new Reservation(new RoomType("Deluxe"), new GuestName("Jane Doe"), DateTime.Now.AddDays(2), DateTime.Now.AddDays(7));
        reservation.ModifyReservation(DateTime.Now.AddDays(3), DateTime.Now.AddDays(8));
        Assert.AreEqual(DateTime.Now.AddDays(3).Date, reservation.CheckInDate.Date);
        Assert.AreEqual(DateTime.Now.AddDays(8).Date, reservation.CheckOutDate.Date);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Reservation_ShouldThrowException_WhenModifyingCancelledReservation()
    {
        var reservation = new Reservation(new RoomType("Deluxe"), new GuestName("Jane Doe"), DateTime.Now.AddDays(2), DateTime.Now.AddDays(7));
        reservation.CancelReservation();
        reservation.ModifyReservation(DateTime.Now.AddDays(3), DateTime.Now.AddDays(8));
    }

    [TestMethod]
    public void Reservation_ShouldCancelCorrectly()
    {
        var reservation = new Reservation(new RoomType("Deluxe"), new GuestName("Jane Doe"), DateTime.Now.AddDays(2), DateTime.Now.AddDays(7));
        reservation.CancelReservation();
        Assert.ThrowsException<InvalidOperationException>(() => reservation.CancelReservation());
    }
}
