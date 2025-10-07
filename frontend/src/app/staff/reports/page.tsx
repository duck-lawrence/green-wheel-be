"use client"

import React, { useMemo } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { ButtonStyled, InputStyled, TextareaStyled, FormStyled } from "@/components"
import { EnumPicker } from "@/components/modules/EnumPicker"
import { useProfileStore } from "@/hooks"

enum StaffReportType {
    Technical = 1,
    Payment = 2,
    Other = 3
}

const REPORT_TYPE_VALUES = Object.values(StaffReportType).filter(
    (value): value is StaffReportType => typeof value === "number"
)

export default function StaffReportsPage() {
    const { t } = useTranslation()
    const user = useProfileStore((state) => state.user)

    const staffName = useMemo(() => {
        if (!user) return ""
        const nameParts = [user.firstName, user.lastName].filter(Boolean)
        return nameParts.join(" ").trim()
    }, [user])

    const validationSchema = useMemo(
        () =>
            Yup.object({
                staffName: Yup.string()
                    .trim()
                    .required(t("staff.report_form_validation_staff_name_required")),
                reportType: Yup.number()
                    .oneOf(
                        REPORT_TYPE_VALUES,
                        t("staff.report_form_validation_report_type_required")
                    )
                    .required(t("staff.report_form_validation_report_type_required")),
                title: Yup.string()
                    .trim()
                    .required(t("staff.report_form_validation_title_required"))
                    .max(100, t("staff.report_form_validation_title_max"))
            }),
        [t]
    )

    const reportTypeLabels = useMemo(
        () =>
            ({
                [StaffReportType.Technical]: t("staff.report_form_type_options_technical"),
                [StaffReportType.Payment]: t("staff.report_form_type_options_payment"),
                [StaffReportType.Other]: t("staff.report_form_type_options_other")
            }) satisfies Record<StaffReportType, string>,
        [t]
    )

    const formik = useFormik({
        enableReinitialize: true,
        initialValues: {
            staffName,
            reportType: null as StaffReportType | null,
            title: "",
            description: ""
        },
        validationSchema,
        onSubmit: (values, helpers) => {
            console.log("[StaffReport] submit payload", values)
            toast.success(t("staff.report_form_success_message"))
            helpers.resetForm({
                values: {
                    staffName,
                    reportType: null,
                    title: "",
                    description: ""
                }
            })
            helpers.setSubmitting(false)
        }
    })

    const handleStaffNameChange = (value: string) => {
        formik.setFieldValue("staffName", value)
    }

    return (
        <div className="mx-auto w-full max-w-3xl rounded-2xl bg-white shadow-sm px-6 py-8">
            <div className="text-3xl mb-3 px-4 font-bold">
                <p>{t("staff.report_page_title")}</p>
            </div>
            <p className="px-4 text-sm text-gray-500 mb-6">{t("staff.report_page_description")}</p>

            <FormStyled onSubmit={formik.handleSubmit} className="space-y-6 items-center">
                <div className="grid gap-4 md:grid-cols-[2fr_1fr]">
                    <InputStyled
                        className="w-full"
                        label={t("staff.report_form_staff_name_label")}
                        value={formik.values.staffName}
                        onValueChange={handleStaffNameChange}
                        onBlur={() => formik.setFieldTouched("staffName", true)}
                        isReadOnly
                        isInvalid={Boolean(formik.touched.staffName && formik.errors.staffName)}
                        errorMessage={formik.errors.staffName}
                    />

                    <div>
                        <EnumPicker<StaffReportType>
                            label={t("staff.report_form_report_type_label")}
                            value={formik.values.reportType}
                            onChange={(value) => {
                                formik.setFieldValue("reportType", value)
                                formik.setFieldTouched("reportType", true, false)
                            }}
                            labels={reportTypeLabels}
                            className="w-full"
                        />
                        {formik.touched.reportType && formik.errors.reportType ? (
                            <p className="mt-1 text-sm text-danger">{formik.errors.reportType}</p>
                        ) : null}
                    </div>

                    <div className="md:col-span-2">
                        <InputStyled
                            className="w-full"
                            label={t("staff.report_form_title_label")}
                            placeholder={t("staff.report_form_title_placeholder")}
                            value={formik.values.title}
                            onValueChange={(value) => formik.setFieldValue("title", value)}
                            onBlur={() => formik.setFieldTouched("title", true)}
                            isInvalid={Boolean(formik.touched.title && formik.errors.title)}
                            errorMessage={formik.errors.title}
                        />
                    </div>

                    <div className="md:col-span-2">
                        <TextareaStyled
                            className="w-full"
                            label={t("staff.report_form_description_label")}
                            placeholder={t("staff.report_form_description_placeholder")}
                            value={formik.values.description}
                            onValueChange={(value) => formik.setFieldValue("description", value)}
                            onBlur={() => formik.setFieldTouched("description", true)}
                            minRows={6}
                        />
                    </div>
                </div>

                <div className="flex justify-center">
                    <ButtonStyled
                        type="submit"
                        color="primary"
                        className="flex min-w-30 items-center justify-center px-6 py-3 text-white"
                        isDisabled={!formik.isValid || formik.isSubmitting}
                    >
                        {t("staff.report_form_submit")}
                    </ButtonStyled>
                </div>
            </FormStyled>
        </div>
    )
}
