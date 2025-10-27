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

export type Category = {
  id: number;
  name: string;
};

export type Location = {
  id: number;
  name: string;
};

export type FilterActivitySessionResponse = {
  id: number;
  activityName?: string;
  locationName?: string;
  categoryName?: string;
  description?: string;
  startUtc?: string;
  endUtc?: string;
  imageUrl?: string;
  capacity?: number;
  isCanceled?: boolean;
};

export type Activity = {
  id: number;
  name: string;
  description: string;
  duration: number;
  location: string;
}

export type ActivityResponse = {
  id: number;
  name: string;
  description: string;
  duration: number;
  location: string;
  imageUrl: string;
  price: number;
  isActive: boolean;
  isOutdoor?: boolean;
};

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

export type CreateActivityRequest = {
  id: number;
  name: string;
  description: string;
  categoryId: number;
  standardDuration: string; // TimeSpan format: "HH:mm:ss"
  imageUrl: string;
  price: number;
  isActive: boolean;
  isOutdoor: boolean;
}

export type CreateSessionRequest = {
  activityId: number;
  locationId: number;
  startUtc: string;
  endUtc: string;
  capacity: number;
}
