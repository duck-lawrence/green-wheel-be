"use client"
import React, { useMemo } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import { CalendarDateTime, now, getLocalTimeZone, fromDate } from "@internationalized/date"
import { ButtonStyled, AutocompleteStyle, DateTimeStyled } from "@/components"
import { useTranslation } from "react-i18next"
import { AutocompleteItem } from "@heroui/react"
import { locals } from "@/data/local"
import { MapPinAreaIcon } from "@phosphor-icons/react"

export function FilterVehicleRental({
    onFilterChange
}: {
    onFilterChange: (stationId: string, start: string, end: string) => void
}) {
    const { t } = useTranslation()
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
                stationId: Yup.string().required(t("vehicle_model.pick_station")),
                start: Yup.mixed<CalendarDateTime>()
                    .required(t("vehicle_model.pick_time_car"))
                    .test("is-valid-start", t("validate.date_received"), (value) => {
                        if (!value) return false
                        const today = now(getLocalTimeZone())
                        return (
                            value.compare(today) >= 0 &&
                            value.hour >= MIN_HOUR &&
                            value.hour < MAX_HOUR
                        )
                    }),
                end: Yup.mixed<CalendarDateTime>()
                    .required(t("validate.date_return"))
                    .test("is-after-start", t("validate.valid_date"), function (value) {
                        const { start } = this.parent
                        return value && start && value.compare(start) > 0
                    })
                    .test("is-valid-end", t("validate.time_return"), (value) => {
                        return value && value.hour >= MIN_HOUR && value.hour < MAX_HOUR
                    })
            }),
        [t]
    )

    //  useFormik
    const formik = useFormik({
        initialValues: {
            stationId: "",
            start: initialStart,
            end: initialStart.add({ hours: 1 })
        },
        validationSchema: bookingSchema,
        onSubmit: (values) => {
            console.log("Booking values item:", {
                stationId: values.stationId,
                start: values.start.toDate(getLocalTimeZone()).toISOString(),
                end: values.end.toDate(getLocalTimeZone()).toISOString()
            })

            onFilterChange(
                values.stationId,
                values.start.toDate(getLocalTimeZone()).toISOString(),
                values.end.toDate(getLocalTimeZone()).toISOString()
            )
        }
    })

    return (
        <>
            <form
                onSubmit={(e) => {
                    if (formik.isSubmitting) {
                        e.preventDefault()
                        return
                    }
                    formik.handleSubmit(e)
                }}
                className="flex gap-6 pt-6 pb-6 justify-center items-center border border-gray-300 rounded-4xl shadow-2xl max-w-[1500px] bg-[#F4F4F4]"
            >
                {/* ĐỊA ĐIỂM */}
                <div className="flex flex-col h-14">
                    <AutocompleteStyle
                        label={t("vehicle_model.station")}
                        items={locals}
                        startContent={<MapPinAreaIcon className="text-xl" />}
                        value={formik.values.stationId}
                        // onChange={(val) => formik.setFieldValue("station", val)}
                        onSelectionChange={(key) => formik.setFieldValue("stationId", key)}
                        className="max-w-60 h-20 mr-0"
                    >
                        {locals &&
                            locals.map((item) => (
                                <AutocompleteItem key={item.key}>{item.label}</AutocompleteItem>
                            ))}
                    </AutocompleteStyle>
                    {formik.touched.stationId && typeof formik.errors.stationId === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.stationId}</div>
                    )}
                </div>

                {/* START */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle_model.start_date_time")}
                        value={formik.values.start}
                        onChange={(val) => formik.setFieldValue("start", val)}
                    />
                    {formik.touched.start && typeof formik.errors.start === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.start}</div>
                    )}
                </div>

                {/* END */}
                <div className="flex flex-col h-14">
                    <DateTimeStyled
                        label={t("vehicle_model.end_date_time")}
                        value={formik.values.end}
                        onChange={(val) => formik.setFieldValue("end", val)}
                    />
                    {formik.touched.end && typeof formik.errors.end === "string" && (
                        <div className="text-red-500 text-sm mt-1">{formik.errors.end}</div>
                    )}
                </div>

                <div className="flex justify-center items-center mt-0">
                    <ButtonStyled type="submit" color="primary" className="w-40 h-13.5">
                        {t("vehicle_model.search_car")}
                    </ButtonStyled>
                </div>
            </form>
        </>
    )
}
