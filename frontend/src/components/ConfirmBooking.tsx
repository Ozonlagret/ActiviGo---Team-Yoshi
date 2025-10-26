import { useState, type ChangeEvent, type FormEvent } from "react";
import { useParams } from "react-router-dom";

interface Booking {
  userId: string;
  activitySessionId: number;
  bookingTimeUtc: Date;
  status: string;
}

  export default function BookingsView() {
  const { ActivitySessionId } = useParams<{ activitySessionId: string }>();

  const now = new Date();
  const bookingUtc = new Date(Date.UTC(
    now.getUTCFullYear(),
    now.getUTCMonth(),
    now.getUTCDate(),
    now.getUTCHours(),
    now.getUTCMinutes(),
    now.getUTCSeconds()
  ))

  const [booking, setBooking] = useState<Booking>({
    userId: "",
    activitySessionId: Number(ActivitySessionId),
    bookingTimeUtc: bookingUtc,
    status: "pending",
  });

  const [message, setMessage] = useState<string>("");

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setBooking({
      ...booking,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      const response = await fetch("http://localhost:7164/api/bookings", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(booking),
      });

      if (response.ok) {
        setMessage("Booking created successfully!");
        setBooking({
          ...booking,
          userId: "",
          bookingTimeUtc: bookingUtc,
          status: "pending",
        });
      } else {
        const err = await response.text();
        setMessage(`Error: ${err}`);
      }
    } catch (error) {
      if (error instanceof Error) 
        setMessage(`Network error: ${error.message}`);
      else 
        setMessage("Unknown error occurred.");
    }
  };

  return (
    <div className="booking-info">
      <h2>Create Booking</h2>
        <input
          type="text"
          name="hairdresserId"
          placeholder="Hairdresser ID"
          value={booking.activitySessionId}
          onChange={handleChange}
          className="border p-2 rounded"
        />

        <input
          type="text"
          name="treatmentId"
          placeholder="Treatment ID"
          value={booking.bookingTimeUtc}
          onChange={handleChange}
          className="border p-2 rounded"
        />

        <input
          type="datetime-local"
          name="date"
          value={booking.status}
          onChange={handleChange}
          className="border p-2 rounded"
        />

        <button
          type="submit"
          className="bg-blue-500 text-white p-2 rounded hover:bg-blue-600"
        >
          Submit Booking
        </button>

      {message && <p className="mt-4">{message}</p>}
    </div>
  );
}