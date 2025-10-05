"use client"
import React, { useEffect, useMemo } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import {
    CalendarDateTime,
    now,
    getLocalTimeZone,
    fromDate,
    parseAbsolute
} from "@internationalized/date"
import { useTranslation } from "react-i18next"
import { AutocompleteItem, Spinner } from "@heroui/react"
import { MapPinAreaIcon } from "@phosphor-icons/react"
import { ButtonStyled, AutocompleteStyle, DateTimeStyled } from "@/components"
import { useBookingFilterStore, useGetAllStations, useGetAllVehicleSegments } from "@/hooks"
import { BackendError } from "@/models/common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import toast from "react-hot-toast"

export function FilterVehicleRental() {
    const { t } = useTranslation()
    const {
        data: stations,
        isLoading: isGetStationsLoading,
        error: getStationsError,
        isError: isGetStationsError
    } = useGetAllStations()
    const {
        data: vehicleSegments,
        isLoading: isGetVehicleSegmentsLoading,
        error: getVehicleSegmentsError,
        isError: isGetVehicleSegmentsError
    } = useGetAllVehicleSegments()

    // manage filter store
    const stationId = useBookingFilterStore((s) => s.stationId)
    const segmentId = useBookingFilterStore((s) => s.segmentId)
    const startDate = useBookingFilterStore((s) => s.startDate)
    const endDate = useBookingFilterStore((s) => s.endDate)
    const setStationId = useBookingFilterStore((s) => s.setStationId)
    const setSegmentId = useBookingFilterStore((s) => s.setSegmentId)
    const setStartDate = useBookingFilterStore((s) => s.setStartDate)
    const setEndDate = useBookingFilterStore((s) => s.setEndDate)

    // set up date time
    const MIN_HOUR = 7
    const MAX_HOUR = 17
    const zonedNow = fromDate(new Date(), getLocalTimeZone())
    const nowTime = new CalendarDateTime(
        zonedNow.year,
        zonedNow.month,
        zonedNow.day,
        zonedNow.hour,
        zonedNow.minute,
        zonedNow.second
    )
    const initialStart =
        nowTime.hour >= MAX_HOUR
            ? nowTime.add({ days: 1 }).set({ hour: MIN_HOUR, minute: 0, second: 0 })
            : nowTime

    //  Validation schema
    const bookingSchema = useMemo(
        () =>
            Yup.object().shape({
                stationId: Yup.string().required(t("vehicle.pick_station")),
                startDate: Yup.mixed<CalendarDateTime>()
                    .required(t("vehicle.pick_time_car"))
                    .test("is-valid-startDate", t("validate.date_received"), (value) => {
                        if (!value) return false
                        const today = now(getLocalTimeZone())
                        return (
                            value.compare(today) >= 0 &&
                            value.hour >= MIN_HOUR &&
                            value.hour < MAX_HOUR
                        )
                    }),
                endDate: Yup.mixed<CalendarDateTime>()
                    .required(t("validate.date_return"))
                    .test("is-after-startDate", t("validate.valid_date"), function (value) {
                        const { startDate } = this.parent
                        return value && startDate && value.compare(startDate) > 0
                    })
                    .test("is-valid-endDate", t("validate.time_return"), (value) => {
                        return value && value.hour >= MIN_HOUR && value.hour < MAX_HOUR
                    })
            }),
        [t]
    )

    //  useFormik
    const formik = useFormik({
        initialValues: {
            stationId: stationId,
            segmentId: segmentId,
            startDate: (startDate && parseAbsolute(startDate, getLocalTimeZone())) || initialStart,
            endDate:
                (endDate && parseAbsolute(endDate, getLocalTimeZone())) ||
                initialStart.add({ hours: 1 })
        },
        validationSchema: bookingSchema,
        onSubmit: (values) => {
            console.log("Booking values item:", {
                stationId: values.stationId,
                segmentId: values.segmentId,
                startDate: values.startDate.toDate(getLocalTimeZone()).toISOString(),
                endDate: values.endDate.toDate(getLocalTimeZone()).toISOString()
            })
        }
    })

    useEffect(() => {
        if (!stationId && !isGetStationsLoading && stations!?.length > 0) {
            formik.values.stationId = stations![0].id
            setStationId(stations![0].id)
        }
    }, [formik.values, isGetStationsLoading, setStationId, stationId, stations])

    useEffect(() => {
        if (isGetStationsError && getStationsError) {
            const error = getStationsError as BackendError
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    }, [isGetStationsError, getStationsError, t])

    useEffect(() => {
        if (isGetVehicleSegmentsError && getVehicleSegmentsError) {
            const error = getVehicleSegmentsError as BackendError
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    }, [isGetVehicleSegmentsError, getVehicleSegmentsError, t])

    if (
        isGetStationsLoading ||
        isGetStationsError ||
        isGetVehicleSegmentsLoading ||
        isGetVehicleSegmentsError
    )
        return <Spinner />

    return (
        <>
            <form
                onSubmit={formik.handleSubmit}
                className="flex gap-6 px-5 pt-3 pb-9 justify-center items-center 
                    border border-gray-300 rounded-4xl shadow-2xl min-w-fit bg-secondary"
            >
                <div className="flex flex-col h-14">
                    <AutocompleteStyle
                        label={t("vehicle.station")}
                        items={stations}
                        startContent={<MapPinAreaIcon className="text-xl" />}
                        selectedKey={formik.values.stationId}
                        onSelectionChange={(id) => {
                            formik.setFieldValue("stationId", id)
                            setStationId(id as string | null)
                        }}
                        className="max-w-60 h-20 mr-0"
                        isClearable={false}
                    >
                        {(stations ?? []).map((item) => (
                            <AutocompleteItem key={item.id}>{item.name}</AutocompleteItem>
                        ))}
                    </AutocompleteStyle>
                    <div className="text-sm mt-1 ml-4">
                        {
                            stations?.find((station) => station.id === formik.values.stationId)
                                ?.address
                        }
                    </div>
                    {formik.touched.stationId && typeof formik.errors.stationId === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.stationId}</div>
                    )}
                </div>

                <div className="flex flex-col h-14">
                    <AutocompleteStyle
                        label={t("vehicle.segment")}
                        items={vehicleSegments}
                        startContent={<MapPinAreaIcon className="text-xl" />}
                        value={formik.values.segmentId || ""}
                        // onChange={(val) => formik.setFieldValue("segment", val)}
                        onSelectionChange={(id) => {
                            formik.setFieldValue("segmentId", id)
                            setSegmentId(id as string | null)
                        }}
                        className="max-w-40 h-20 mr-0"
                    >
                        {(vehicleSegments ?? []).map((item) => (
                            <AutocompleteItem key={item.id}>{item.name}</AutocompleteItem>
                        ))}
                    </AutocompleteStyle>
                    {formik.touched.segmentId && typeof formik.errors.segmentId === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.segmentId}</div>
                    )}
                </div>

                {/* STARTDate */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle.start_date_time")}
                        value={formik.values.startDate as CalendarDateTime}
                        onChange={(val) => {
                            formik.setFieldValue("startDate", val)
                            setStartDate(val.toDate(getLocalTimeZone()).toISOString())
                        }}
                    />
                    {formik.touched.startDate && typeof formik.errors.startDate === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.startDate}</div>
                    )}
                </div>

                {/* ENDDate */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle.end_date_time")}
                        value={formik.values.endDate as CalendarDateTime}
                        onChange={(val) => {
                            formik.setFieldValue("endDate", val)
                            setEndDate(val.toDate(getLocalTimeZone()).toISOString())
                        }}
                    />
                    {formik.touched.endDate && typeof formik.errors.endDate === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.endDate}</div>
                    )}
                </div>

                <div className="flex justify-center items-center mt-0">
                    <ButtonStyled type="submit" color="primary" className="w-40 h-13.5">
                        {t("vehicle.search_car")}
                    </ButtonStyled>
                </div>
            </form>
        </>
    )
}
