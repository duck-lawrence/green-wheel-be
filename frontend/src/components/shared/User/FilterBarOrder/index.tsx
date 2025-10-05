"use client"
import { useFormik } from "formik"
import React from "react"
import * as Yup from "yup"
import { OrderStatus } from "@/constants/enum"
import { ButtonStyled, DateRangePickerStyled } from "@/components/styled"
import dayjs from "dayjs"
import { VehicalModelPicker } from "../VehicalModelPicker"
import { EnumPicker } from "@/components/modules/EnumPicker"
import { OrderStatusLabels } from "@/constants/labels"

export function FillterBarOrder({ onFilterChange }: { onFilterChange: () => void }) {
    const formik = useFormik({
        initialValues: {
            vehicalModel: "",
            status: OrderStatus.All,
            start: "",
            end: ""
        },
        validationSchema: Yup.object().shape({
            vehicalModel: Yup.string().required("Please pick vehicle model"),
            status: Yup.number().required("Please choose status"),
            start: Yup.string().required("Choose start date"),
            end: Yup.string().required("Choose end date")
        }),
        onSubmit: (values) => {
            console.log("Payload gửi API:", values)
            onFilterChange()
        }
    })

    return (
        <form
            onSubmit={(e) => {
                if (formik.isSubmitting) {
                    e.preventDefault()
                    return
                }
                formik.handleSubmit(e)
            }}
            className="flex gap-4"
        >
            <VehicalModelPicker
                value={formik.values.vehicalModel}
                onChange={(val) => formik.setFieldValue("vehicalModel", val)}
            />

            <EnumPicker
                value={formik.values.status}
                onChange={(val) => formik.setFieldValue("status", val)}
                labels={OrderStatusLabels}
                label="Status"
                className="max-w-55 h-14"
            />

            <DateRangePickerStyled
                label="Pick start date and end date"
                onChange={(val) => {
                    if (!val) {
                        formik.setFieldValue("start", null)
                        formik.setFieldValue("end", null)
                        return
                    }

                    const startStr = val.start
                        ? dayjs(val.start.toDate("Asia/Ho_Chi_Minh")).format("YYYY-MM-DD")
                        : ""
                    const endStr = val.end
                        ? dayjs(val.end.toDate("Asia/Ho_Chi_Minh")).format("YYYY-MM-DD")
                        : ""

                    formik.setFieldValue("start", startStr)
                    formik.setFieldValue("end", endStr)
                }}
            />
            <ButtonStyled type="submit" className="h-[56] w-40 ml-10">
                Search
            </ButtonStyled>
        </form>
    )
}
