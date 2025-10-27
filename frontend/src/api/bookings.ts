import { api } from "./client";

export type BookSessionRequest = {
	activitySessionId: number;
};

export type BookingResponse = {
	id: number;
	activitySessionId: number;
	activityName: string;
	bookingTimeUtc: string; // ISO
	status: string; // "Active" | "Canceled" | "Completed" etc.
	startDateUtc: string; // ISO
	endDateUtc: string;   // ISO
};

export type MyBookingsResponse = {
	upcoming: BookingResponse[];
	past: BookingResponse[];
	canceled: BookingResponse[];
};

export async function bookSession(activitySessionId: number) {
	const body: BookSessionRequest = { activitySessionId };
	const { data } = await api.post<BookingResponse>("/api/bookings", body);
	return data;
}

export async function getMyBookings() {
	const { data } = await api.get<MyBookingsResponse>("/api/bookings/my");
	return data;
}

export async function cancelBooking(id: number) {
	await api.post(`/api/bookings/${id}/cancel`);
}

