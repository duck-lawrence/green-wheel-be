// src/store/useBookingStore.ts
import Vehicle from "@/models/vehicle/vehicle"
// import { shallow } from "@/utils/helpers/shallow"
import { create } from "zustand"
import { useShallow } from "zustand/react/shallow"
type BookingState = {
    station: string | null
    start: string | null
    end: string | null
    filteredVehicles: Vehicle[]
    // setBooking: (data: Partial<BookingState>) => void
    // clearBooking: () => void
}

interface BookingActions {
    setBookingInfo: (station: string, start: string, end: string) => void
    setFilteredVehicles: (vehicles: Vehicle[]) => void
    clearBooking: () => void
}

export const useBookingStore = create<BookingState & BookingActions>((set) => ({
    station: null,
    start: null,
    end: null,
    filteredVehicles: [],

    setBookingInfo: (station, start, end) => set({ station, start, end }),
    setFilteredVehicles: (vehicles) => set({ filteredVehicles: vehicles }),
    clearBooking: () =>
        set({
            station: null,
            start: null,
            end: null,
            filteredVehicles: []
        })
}))

// ----------------------
// Selectors (để tối ưu render)
// ----------------------
export const useBookingInfo = () => {
    const selector = useShallow((s: BookingState) => ({
        station: s.station,
        start: s.start,
        end: s.end
    }))
    return useBookingStore(selector)
}

// export const useFilteredVehicles = () => useBookingStore((s) => s.filteredVehicles)

// export const useBookingActions = () =>
//     useBookingStore(
//         (s) => ({
//             setBookingInfo: s.setBookingInfo,
//             setFilteredVehicles: s.setFilteredVehicles,
//             clearBooking: s.clearBooking
//         }),
//         shallow
//     )

// const savedBooking =
//     typeof window !== "undefined" ? JSON.parse(sessionStorage.getItem("booking") || "{}") : {}

// export const useBookingStore = create<BookingState>((set) => ({
//     station: savedBooking.station ?? null,
//     start: savedBooking.start ?? null,
//     end: savedBooking.end ?? null,

//     setBooking: (data) => {
//         set((state) => {
//             const newState = { ...state, ...data }
//             if (typeof window !== "undefined") {
//                 sessionStorage.setItem("booking", JSON.stringify(newState))
//             }
//             return newState
//         })
//     },

//     clearBooking: () => {
//         if (typeof window !== "undefined") {
//             sessionStorage.removeItem("booking")
//         }
//         set({ station: null, start: null, end: null })
//     }
// }))
