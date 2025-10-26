export type ActivitySession = {
  id?: number;
  activityId?: number;
  activityName?: string;
  locationId?: number;
  startUtc?: string;
  endUtc?: string;
  capacity?: number;
  isCanceled?: boolean;
  bookedCount?: number;
};

export type FilterActivitySessionResponse = {
  sessionId?: number;
  name?: string;
  location?: string;
  category?: string;
  description?: string;
  startUtc?: string;
  endUtc?: string;
  imageUrl?: string;
  capacity?: number;
  IsCanceled?: boolean;
};

export type Activity = {
  id: number;
  name: string;
  description: string;
  duration: number;
  location: string;
}

export type BookingResponse = {
  id: number;
  userId: string;
  activitySessionId: number;
  status: BookingStatus;
}

export type BookSessionRequest = {
  userId: string;
  activitySessionId: number;
}

type BookingStatus = "Active" | "Canceled" | "Completed";