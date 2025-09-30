"use client"
import { useFormik } from "formik"
import React from "react"
import * as Yup from "yup"

import { OrderStatus } from "@/constants/enum"

import { ButtonStyled } from "@/components/styled"
import dayjs from "dayjs"
import { VehicalModelPicker } from "../VehicalModelPicker"
import { StatusOrderPicker } from "../StatusPicker"

export function FillterBarOrder({ onFilterChange }) {
    const formik = useFormik({
        initialValues: {
            vehicalModel: "",
            status: OrderStatus.All,
            start: "",
            end: ""
        },
        validationSchema: Yup.object().shape({
            vehicalModel: Yup.string().required("Please pick vehicle model"),
            status: Yup.string().required("Please choose status"),
            start: Yup.string().required("Choose start date"),
            end: Yup.string().required("Choose end date")
        }),
        onSubmit: (values) => {
            console.log("Payload gửi API:", values)
            onFilterChange(values)
        }
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex gap-4">
            <VehicalModelPicker
                value={formik.values.vehicalModel}
                onChange={(val) => formik.setFieldValue("vehicalModel", val)}
            />

            <StatusOrderPicker
                value={formik.values.status}
                onChange={(val) => formik.setFieldValue("status", val)}
            />

            <DateRangePickerStyled
                onChange={(val) => {
                    if (!val) {
                        formik.setFieldValue("start", "")
                        formik.setFieldValue("end", "")
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
