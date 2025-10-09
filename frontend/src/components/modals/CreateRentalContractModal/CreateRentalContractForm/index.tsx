"use client"

import React, { useState, useEffect, useCallback, useMemo } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import { useTranslation } from "react-i18next"
import Link from "next/link"

import {
    useBookingFilterStore,
    useGetAllStations,
    useGetMe,
    useCreateRentalContract,
    useDay
} from "@/hooks"
import { ButtonStyled, InputStyled, ImageStyled, TextareaStyled } from "@/components"
import { Spinner } from "@heroui/react"
import toast from "react-hot-toast"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { BackendError } from "@/models/common/response"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
import { StationViewRes } from "@/models/station/schema/response"
import { formatCurrency } from "@/utils/helpers/currency"
import { CheckboxStyled } from "@/components/styled/CheckboxStyled"

type FormValues = {
    fullName: string
    phone: string
    email: string
    stationId: string
    note: string
    // paymentMethod: PaymentMethod
    agreeTerms: boolean
    agreeDataPolicy: boolean
}

export const CreateRentalContractForm = ({
    onSuccess,
    totalDays,
    totalPrice,
    modelViewRes
}: {
    onSuccess?: () => void
    totalDays: number
    totalPrice: number
    modelViewRes: VehicleModelViewRes
}) => {
    const { t } = useTranslation()
    const { formatDateTime } = useDay({ defaultFormat: "DD-MM-YYYY HH:mm" })
    const [mounted, setMounted] = useState(false)
    const createContract = useCreateRentalContract({ onSuccess })
    const { data: user, isLoading: isUserLoading, error: userError } = useGetMe()
    const {
        data: stations,
        isLoading: isStationsLoading,
        error: stationsError
    } = useGetAllStations()
    const stationId = useBookingFilterStore((s) => s.stationId)
    const startDate = useBookingFilterStore((s) => s.startDate)
    const endDate = useBookingFilterStore((s) => s.endDate)

    const stationMap = useMemo(() => {
        const map = new Map<string, StationViewRes>()
        stations?.forEach((s) => map.set(s.id, s))
        return map
    }, [stations])

    const station = stationMap.get(stationId!)

    const handleCreateContract = useCallback(async () => {
        await createContract.mutateAsync({
            customerId: undefined,
            modelId: modelViewRes.id,
            stationId: stationId!,
            startDate: startDate!,
            endDate: endDate!
        })
    }, [createContract, endDate, modelViewRes.id, startDate, stationId])

    // handle moute
    useEffect(() => {
        setMounted(!isUserLoading && !isStationsLoading)
    }, [isStationsLoading, isUserLoading])

    useEffect(() => {
        if (userError || stationsError) {
            const error = (userError as BackendError) || (stationsError as BackendError)
            toast.error(translateWithFallback(t, error.detail))
            onSuccess?.()
        }
    }, [onSuccess, stationsError, t, userError])

    const initialValues: FormValues = {
        fullName: `${user?.lastName ?? ""} ${user?.firstName ?? ""}`,
        phone: user?.phone ?? "",
        email: user?.email ?? "",
        stationId: stationId || "",
        note: "",
        // paymentMethod: PaymentMethod.Cash,
        agreeTerms: false,
        agreeDataPolicy: false
    }

    const formik = useFormik<FormValues>({
        enableReinitialize: true,
        validateOnMount: true,
        initialValues,
        validationSchema: Yup.object().shape({
            // fullName: Yup.string().required(t("user.full_name_require")),
            // phone: Yup.string()
            //     .matches(PHONE_REGEX, t("user.invalid_phone"))
            //     .required(t("user.phone_require")),
            // email: Yup.string().email(t("user.invalid_email")).required(t("user.email_require")),
            // pickupLocation: Yup.string().required(t("contral_form.pickup_location_require")),
            // stationId: Yup.string().required(t("vehicle_model.pick_station")),
            // note: Yup.string(),
            // paymentMethod: Yup.mixed<PaymentMethod>()
            //     .oneOf(Object.values(PaymentMethod) as PaymentMethod[])
            //     .required(t("contral_form.payment_method_require")),
            agreeTerms: Yup.boolean().oneOf([true], t("contral_form.agree_terms_require")),
            agreeDataPolicy: Yup.boolean().oneOf(
                [true],
                t("contral_form.agree_data_policy_require")
            )
        }),
        onSubmit: handleCreateContract
    })

    // const renterFilled = !!formik.values.fullName?.trim()
    // const emailFilled = !!formik.values.email?.trim()

    return (
        <div className="max-h-[95vh] px-4 sm:px-6 lg:px-8">
            {mounted ? (
                <div className="mx-auto max-w-5xl bg-white rounded-lg ">
                    <div className="text-center border-gray-100">
                        <h2 className="text-3xl font-bold">{t("car_rental.register_title")}</h2>
                    </div>

                    <form onSubmit={formik.handleSubmit} className="px-6 py-4" noValidate>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            {/* Cột trái */}
                            <div>
                                <div className="space-y-4">
                                    <div className="grid grid-cols-2 gap-2">
                                        {/* Họ tên */}
                                        <InputStyled
                                            variant="bordered"
                                            label={t("car_rental.renter_name")}
                                            placeholder={t("car_rental.renter_name_placeholder")}
                                            value={formik.values.fullName}
                                            readOnly={true}
                                        />

                                        {/* Phone */}
                                        <InputStyled
                                            variant="bordered"
                                            label={t("car_rental.phone")}
                                            placeholder={t("car_rental.phone_placeholder")}
                                            type="tel"
                                            inputMode="numeric"
                                            value={formik.values.phone}
                                            readOnly={true}
                                        />

                                        {/* Pickup location */}
                                        <InputStyled
                                            variant="bordered"
                                            label={t("car_rental.pickup_location")}
                                            value={`${station?.name || ""} - ${
                                                station?.address || ""
                                            }`}
                                            readOnly={true}
                                        />

                                        {/* Email */}
                                        <InputStyled
                                            variant="bordered"
                                            label={t("car_rental.email")}
                                            placeholder={t("car_rental.email_placeholder")}
                                            type="email"
                                            value={formik.values.email}
                                            readOnly={true}
                                        />
                                    </div>

                                    {/* Note */}
                                    <TextareaStyled
                                        label={t("car_rental.note")}
                                        placeholder=""
                                        value={formik.values.note}
                                        onValueChange={(v) => formik.setFieldValue("note", v)}
                                        onBlur={() => formik.setFieldTouched("note", true)}
                                        isInvalid={!!(formik.touched.note && formik.errors.note)}
                                        errorMessage={
                                            formik.touched.note ? formik.errors.note : undefined
                                        }
                                        minRows={4}
                                    />

                                    {/* Payment Method */}
                                    {/* <EnumPicker<PaymentMethod>
                                        value={formik.values.paymentMethod}
                                        onChange={(v) => {
                                            formik.setFieldValue("paymentMethod", v)
                                            formik.setFieldTouched("paymentMethod", true)
                                        }}
                                        labels={PaymentMethodLabels}
                                        label={t("car_rental.select_payment_method")}
                                    />
                                    {formik.touched.paymentMethod &&
                                        formik.errors.paymentMethod && (
                                            <p className="text-red-500 text-sm mt-1">
                                                {formik.errors.paymentMethod as string}
                                            </p>
                                        )} */}
                                </div>

                                {/* Policy */}
                                <div className="mt-6 space-y-4">
                                    <CheckboxStyled
                                        id="agreeTerms"
                                        name="agreeTerms"
                                        checked={formik.values.agreeTerms}
                                        isInvalid={
                                            !!(
                                                formik.touched.agreeTerms &&
                                                formik.errors.agreeTerms
                                            )
                                        }
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        className="mt-1"
                                    >
                                        {t("car_rental.agree_terms")}{" "}
                                        <Link href="#" className="text-blue-600 hover:underline">
                                            {t("car_rental.payment_terms")}
                                        </Link>{" "}
                                        {t("car_rental.of_green_wheel")}
                                    </CheckboxStyled>

                                    <CheckboxStyled
                                        id="agreeDataPolicy"
                                        name="agreeDataPolicy"
                                        checked={formik.values.agreeDataPolicy}
                                        isInvalid={
                                            !!(
                                                formik.touched.agreeDataPolicy &&
                                                formik.errors.agreeDataPolicy
                                            )
                                        }
                                        // aria-errormessage={formik.errors.agreeDataPolicy}
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                    >
                                        {t("car_rental.agree_data_policy")}{" "}
                                        <Link href="#" className="text-blue-600 hover:underline">
                                            {t("car_rental.data_sharing_terms")}
                                        </Link>{" "}
                                        {t("car_rental.of_green_wheel")}
                                    </CheckboxStyled>
                                </div>
                            </div>

                            {/* Cột phải: Thông tin xe */}
                            <div>
                                <div className="bg-gray-50 p-4 rounded-lg">
                                    <div className="flex items-center space-x-4">
                                        <div
                                            className="
                                                relative overflow-hidden rounded-md
                                                w-40 h-28
                                                sm:w-48 sm:h-32
                                                md:w-56 md:h-36
                                            "
                                        >
                                            <ImageStyled
                                                src={modelViewRes.imageUrl || ""}
                                                alt={t("car_rental.vehicle")}
                                                className="absolute inset-0 w-full h-full object-contain"
                                                width={800}
                                                height={520}
                                            />
                                        </div>
                                        <div>
                                            <h3 className="text-lg font-medium">
                                                {modelViewRes.name}
                                            </h3>
                                        </div>
                                    </div>

                                    <div className="mt-4 bg-blue-50 p-4 rounded-lg">
                                        <div className="flex justify-between items-center">
                                            <h4 className="font-medium">{station?.name}</h4>
                                            <div>{station?.address}</div>
                                        </div>
                                        <div className="mt-2 flex items-center">
                                            <span>
                                                {`${totalDays} ${t(
                                                    "car_rental.days"
                                                )} • ${formatDateTime({
                                                    date: startDate!
                                                })} → ${formatDateTime({ date: endDate! })}`}
                                            </span>
                                        </div>
                                        {/* <div className="mt-1">
                                            <span className="text-sm">
                                                Hình thức thuê: Theo ngày
                                            </span>
                                        </div> */}
                                    </div>

                                    <div className="mt-4">
                                        <h4 className="font-medium text-center">
                                            {t("car_rental.detail_table")}
                                        </h4>
                                        {/* <div className="mt-2 space-y-2">
                                            <div className="flex justify-between">
                                                <span>{t("car_rental.listed_fee")}</span>
                                                <span>{formatCurrency(1)}</span>
                                            </div>
                                            <div className="border-t pt-2 flex justify-between font-medium">
                                                <span>{t("car_rental.total")}</span>
                                                <span>{formatCurrency(1)}</span>
                                            </div>
                                            <div className="flex justify-between">
                                                <span>{t("car_rental.deposit")}</span>
                                                <span>{formatCurrency(1)}</span>
                                            </div>
                                        </div> */}
                                        <div className="rounded-xl bg-neutral-50 p-4">
                                            <div className="flex items-center justify-between text-sm">
                                                <span>{t("vehicle_model.unit_price")}</span>
                                                <span className="font-medium">
                                                    {formatCurrency(modelViewRes.costPerDay)}
                                                </span>
                                            </div>
                                            <div className="mt-2 flex items-center justify-between text-sm">
                                                <span>{t("vehicle_model.number_of_days")}</span>
                                                <span className="font-medium">{totalDays}</span>
                                            </div>
                                            <div className="mt-2 flex items-center justify-between text-sm">
                                                <span>{t("vehicle_model.deposit_fee")}</span>
                                                <span className="font-medium">
                                                    {formatCurrency(modelViewRes.depositFee)}
                                                </span>
                                            </div>

                                            <div className="mt-3 h-px bg-neutral-200" />
                                            <div className="mt-3 flex items-center justify-between text-base font-semibold">
                                                <span>{t("vehicle_model.temporary_total")}</span>
                                                <span className="text-emerald-700">
                                                    {formatCurrency(totalPrice)}
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    {/* <div className="mt-6 border-t pt-4">
                                        <div className="flex justify-between items-center">
                                            <div>
                                                <span className="font-medium">
                                                    {t("car_rental.payment")}
                                                </span>
                                                <span className="text-xs text-gray-500 block">
                                                    {t("car_rental.price_includes_vat")}
                                                </span>
                                            </div>
                                            <span className="text-2xl font-bold text-green-500">
                                                {formatCurrency(totalPayment)}
                                            </span>
                                        </div>
                                    </div> */}
                                </div>
                            </div>
                        </div>

                        {/* Submit */}
                        <div className="mt-8 text-center">
                            <ButtonStyled
                                type="submit"
                                isDisabled={!formik.isValid || formik.isSubmitting}
                                isLoading={formik.isSubmitting}
                                color={
                                    !formik.isValid || formik.isSubmitting ? "default" : "success"
                                }
                                variant={!formik.isValid || formik.isSubmitting ? "flat" : "solid"}
                                className="px-8 py-2 rounded-md"
                            >
                                {t("vehicle_model.create_rental_request")}
                            </ButtonStyled>
                        </div>
                    </form>
                </div>
            ) : (
                <Spinner />
            )}
        </div>
    )
}
