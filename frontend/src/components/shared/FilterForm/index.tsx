"use client"
import React, { useEffect, useMemo } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import { CalendarDateTime, fromDate } from "@internationalized/date"
import { useTranslation } from "react-i18next"
import { AutocompleteItem, Spinner } from "@heroui/react"
import { MapPinAreaIcon } from "@phosphor-icons/react"
import { ButtonStyled, AutocompleteStyle, DateTimeStyled } from "@/components"
import { useBookingFilterStore, useDay, useGetAllStations, useGetAllVehicleSegments } from "@/hooks"
import { BackendError } from "@/models/common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import toast from "react-hot-toast"
import {
    DEFAULT_DATE_TIME_FORMAT,
    DEFAULT_TIMEZONE,
    MAX_HOUR,
    MIN_HOUR
} from "@/constants/constants"
import dayjs from "dayjs"

export function FilterVehicleRental() {
    const { t } = useTranslation()
    const { formatDateTime, toCalenderDateTime } = useDay({})
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

    // setup date time
    const { minStartDate, minEndDate } = useMemo(() => {
        const zonedNow = fromDate(new Date(), DEFAULT_TIMEZONE)
        const nowTime = new CalendarDateTime(
            zonedNow.year,
            zonedNow.month,
            zonedNow.day,
            zonedNow.hour,
            zonedNow.minute,
            zonedNow.second
        )

        const initialStart =
            nowTime.hour + 3 >= MAX_HOUR
                ? nowTime.add({ days: 1 }).set({ hour: MIN_HOUR, minute: 0, second: 0 })
                : nowTime.set({ hour: nowTime.hour + 3, second: 0 })

        return {
            minStartDate: initialStart,
            minEndDate: initialStart.add({ days: 1 })
        }
    }, [])

    useEffect(() => {
        if (!startDate)
            setStartDate(
                dayjs(minStartDate.toDate(DEFAULT_TIMEZONE)).format(DEFAULT_DATE_TIME_FORMAT)
            )
        if (!endDate)
            setEndDate(dayjs(minEndDate.toDate(DEFAULT_TIMEZONE)).format(DEFAULT_DATE_TIME_FORMAT))
    }, [endDate, minEndDate, minStartDate, setEndDate, setStartDate, startDate])

    //  Validation schema
    const bookingSchema = useMemo(
        () =>
            Yup.object().shape({
                stationId: Yup.string().required(t("vehicle.pick_station")),
                startDate: Yup.string()
                    .required(t("vehicle_filter.start_date_require"))
                    .test(
                        "is-valid-start-date",
                        t("vehicle_filter.invalid_start_date"),
                        (value) => {
                            const valueDatetime = toCalenderDateTime(value)
                            return (
                                !!valueDatetime &&
                                valueDatetime.compare(minStartDate) >= 0 &&
                                valueDatetime.hour >= MIN_HOUR &&
                                valueDatetime.hour <= MAX_HOUR
                            )
                        }
                    ),
                endDate: Yup.string()
                    .required(t("vehicle_filter.return_date_require"))
                    .test(
                        "is-valid-end-date-range",
                        t("vehicle_filter.invalid_end_date_range"),
                        (value) => {
                            const valueDatetime = toCalenderDateTime(value)
                            return (
                                !!valueDatetime &&
                                valueDatetime.hour >= MIN_HOUR &&
                                valueDatetime.hour <= MAX_HOUR
                            )
                        }
                    )
                    .test("is-valid-min-end-date", t("vehicle_filter.min_end_date"), (value) => {
                        return dayjs(startDate).isBefore(dayjs(value).add(-1, "day"))
                    })
            }),
        [minStartDate, startDate, t, toCalenderDateTime]
    )

    //  useFormik
    const formik = useFormik({
        initialValues: {
            stationId: stationId,
            segmentId: segmentId,
            startDate: startDate || minStartDate.toString(),
            endDate: endDate || minEndDate.toString()
        },
        validationSchema: bookingSchema,
        onSubmit: (values) => {
            console.log("Booking values item:", {
                stationId: values.stationId,
                segmentId: values.segmentId,
                startDate: values.startDate,
                endDate: values.endDate
            })
        }
    })

    // Load station
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

    // Load segment
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
                className="flex gap-6 px-5 pt-3 pb-8 justify-center items-center 
                    border border-gray-300 rounded-4xl shadow-2xl min-w-fit bg-secondary"
            >
                <div className="flex flex-col h-14">
                    <AutocompleteStyle
                        label={t("vehicle.station")}
                        items={stations}
                        startContent={<MapPinAreaIcon className="text-xl" />}
                        selectedKey={formik.values.stationId}
                        errorMessage={formik.errors.stationId}
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
                    {/* {formik.touched.stationId && typeof formik.errors.stationId === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.stationId}</div>
                    )} */}
                </div>

                <div className="flex flex-col h-14">
                    <AutocompleteStyle
                        label={t("vehicle.segment")}
                        items={vehicleSegments}
                        // startContent={<MapPinAreaIcon className="text-xl" />}
                        value={formik.values.segmentId || ""}
                        errorMessage={formik.errors.segmentId}
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
                    {/* {formik.touched.segmentId && typeof formik.errors.segmentId === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.segmentId}</div>
                    )} */}
                </div>

                {/* STARTDate */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle.start_date_time")}
                        value={toCalenderDateTime(formik.values.startDate)}
                        minValue={minStartDate}
                        isInvalid={!!(formik.touched.startDate && formik.errors.startDate)}
                        errorMessage={formik.errors.startDate}
                        onChange={(value) => {
                            if (!value) {
                                formik.setFieldValue("startDate", null)
                                return
                            }

                            const date = formatDateTime({ value })

                            formik.setFieldValue("startDate", date)
                            setStartDate(date)
                        }}
                        onBlur={() => {
                            formik.setFieldTouched("startDate")
                        }}
                    />
                    {/* {formik.touched.startDate && typeof formik.errors.startDate === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.startDate}</div>
                    )} */}
                </div>

                {/* ENDDate */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle.end_date_time")}
                        value={toCalenderDateTime(formik.values.endDate)}
                        minValue={minEndDate}
                        isInvalid={!!(formik.touched.endDate && formik.errors.endDate)}
                        errorMessage={formik.errors.endDate}
                        onChange={(value) => {
                            if (!value) {
                                formik.setFieldValue("endDate", null)
                                return
                            }

                            const date = formatDateTime({ value })

                            formik.setFieldValue("endDate", date)
                            setEndDate(date)
                        }}
                        onBlur={() => {
                            formik.setFieldTouched("endDate")
                        }}
                    />
                    {/* {formik.touched.endDate && typeof formik.errors.endDate === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.endDate}</div>
                    )} */}
                </div>

                <div className="flex justify-center items-center mt-0">
                    <ButtonStyled
                        type="submit"
                        color="primary"
                        // isDisabled={!formik.isValid}
                        className="w-40 h-13.5"
                    >
                        {t("vehicle.search_car")}
                    </ButtonStyled>
                </div>
            </form>
        </>
    )
}
