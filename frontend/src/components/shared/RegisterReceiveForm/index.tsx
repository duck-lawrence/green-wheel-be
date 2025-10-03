"use client"

import React, { useEffect, useMemo, useCallback, useState } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import { useTranslation } from "react-i18next"
import Link from "next/link"

import { useProfileStore, useToken } from "@/hooks"
import {
  ButtonStyled,
  InputStyled,
  LocalFilter,
  EnumPicker,
  ImageStyled,
  TextareaStyled,
} from "@/components"
import { PaymentMethod } from "@/constants/enum"
import { PaymentMethodLabels } from "@/constants/labels"

type FormValues = {
  fullName: string
  phone: string
  email: string
  pickupLocation: string
  note: string
  paymentMethod: PaymentMethod | null
  agreeTerms: boolean
  agreeDataPolicy: boolean
}

export const RegisterReceiveForm = () => {
  const { t } = useTranslation("common")
  const [mounted, setMounted] = useState(false)
  const { user } = useProfileStore()
  const isLoggedIn = useToken((s) => !!s.accessToken)

  useEffect(() => setMounted(true), [])

  const LISTED_FEE = 590_000
  const DEPOSIT = 5_000_000
  const totalPayment = LISTED_FEE + DEPOSIT
  const formatCurrency = useCallback(
    (n: number) => new Intl.NumberFormat("vi-VN").format(n) + "đ",
    []
  )

  const initialValues = useMemo<FormValues>(
    () => ({
      fullName: isLoggedIn && user ? `${user.firstName} ${user.lastName}` : "",
      phone: isLoggedIn && user?.phone ? user.phone : "",
      email: isLoggedIn && user ? user.email : "",
      pickupLocation: "",
      note: "",
      paymentMethod: null,
      agreeTerms: false,
      agreeDataPolicy: false,
    }),
    [isLoggedIn, user]
  )

  const schema = useMemo(
    () =>
      Yup.object({
        fullName: Yup.string().required(t("fullName.required")),
        phone: Yup.string()
          .min(9, t("car_rental.phone_min_length"))
          .matches(/^[0-9]{9,11}$/, t("phone.invalid"))
          .required(t("phone.required")),
        email: Yup.string().email(t("email.invalid")).required(t("email.required")),
        pickupLocation: Yup.string().required(t("pickupLocation.required")),
        note: Yup.string(),
        paymentMethod: Yup.mixed<PaymentMethod>()
          .oneOf(Object.values(PaymentMethod) as PaymentMethod[])
          .required(t("paymentMethod.required")),
        agreeTerms: Yup.boolean().oneOf([true], t("agreeTerms.required")),
        agreeDataPolicy: Yup.boolean().oneOf([true], t("agreeDataPolicy.required")),
      }),
    [t]
  )

  const formik = useFormik<FormValues>({
    enableReinitialize: true,
    validateOnMount: true,
    initialValues,
    validationSchema: schema,
    onSubmit: (values) => {
      console.log("Form values:", values)
      // TODO: call API submit
    },
  })

  const bind = <K extends keyof FormValues>(name: K) => ({
    value: formik.values[name] as any,
    onValueChange: (v: any) => formik.setFieldValue(name, v),
    onBlur: () => formik.setFieldTouched(name, true),
    isInvalid: Boolean(formik.touched[name] && formik.errors[name]),
    errorMessage: formik.touched[name] ? (formik.errors[name] as any) : undefined,
  })

  const renterFilled = !!formik.values.fullName?.trim()
  const emailFilled = !!formik.values.email?.trim()

  return (
    <div className="min-h-screen bg-gray-50 py-10 px-4 sm:px-6 lg:px-8 mt-30">
      {mounted ? (
        <div className="mx-auto max-w-5xl bg-white rounded-lg">
          <div className="flex items-center justify-between border-b border-gray-100 px-6 py-6">
            <h2 className="text-3xl font-bold">{t("car_rental.register_title")}</h2>
          </div>

          <form onSubmit={formik.handleSubmit} className="px-6 pb-8 pt-6" noValidate>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Cột trái */}
              <div>
                <div className="space-y-4">
                  {/* Họ tên */}
                  <InputStyled
                    label={t("car_rental.renter_name")}
                    placeholder={t("car_rental.renter_name_placeholder")}
                    variant="bordered"
                    readOnly={isLoggedIn}
                    classNames={{
                      inputWrapper: renterFilled ? "bg-gray-100 border-gray-200" : undefined,
                      input: renterFilled ? "text-gray-600 placeholder:text-gray-400" : undefined,
                      label: renterFilled ? "text-gray-500" : undefined,
                    }}
                    {...bind("fullName")}
                  />

                  {/* Điện thoại */}
                  <InputStyled
                    label={t("car_rental.phone")}
                    placeholder={t("car_rental.phone_placeholder")}
                    type="tel"
                    inputMode="numeric"
                    onClear={() => formik.setFieldValue("phone", "")}
                    variant="bordered"
                    {...bind("phone")}
                  />

                  {/* Email */}
                  <InputStyled
                    label={t("car_rental.email")}
                    placeholder="wheel@fpt.edu.vn"
                    type="email"
                    readOnly={isLoggedIn}
                    variant="bordered"
                    classNames={{
                      inputWrapper: emailFilled ? "bg-gray-100 border-gray-200" : undefined,
                      input: emailFilled ? "text-gray-600 placeholder:text-gray-400" : undefined,
                      label: emailFilled ? "text-gray-500" : undefined,
                    }}
                    {...bind("email")}
                  />

                  {/* Điểm nhận xe */}
                  <LocalFilter
                    value={formik.values.pickupLocation || null}
                    onChange={(val) => {
                      formik.setFieldValue("pickupLocation", val ?? "")
                      formik.setFieldTouched("pickupLocation", true)
                    }}
                  />
                  {formik.touched.pickupLocation && formik.errors.pickupLocation && (
                    <p className="text-red-500 text-sm mt-1">
                      {formik.errors.pickupLocation as string}
                    </p>
                  )}

                  {/* Ghi chú */}
                  <TextareaStyled
                    label={t("car_rental.note")}
                    placeholder=""
                    minRows={4}
                    {...bind("note")}
                  />

                  {/* Phương thức thanh toán */}
                  <EnumPicker<PaymentMethod>
                    value={formik.values.paymentMethod}
                    onChange={(v) => {
                      formik.setFieldValue("paymentMethod", v)
                      formik.setFieldTouched("paymentMethod", true)
                    }}
                    labels={PaymentMethodLabels}
                    label={t("car_rental.select_payment_method")}
                  />
                  {formik.touched.paymentMethod && formik.errors.paymentMethod && (
                    <p className="text-red-500 text-sm mt-1">
                      {formik.errors.paymentMethod as string}
                    </p>
                  )}
                </div>

                {/* Điều khoản */}
                <div className="mt-6 space-y-3">
                  <div className="flex items-start">
                    <input
                      id="agreeTerms"
                      name="agreeTerms"
                      type="checkbox"
                      checked={formik.values.agreeTerms}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      className="mt-1"
                    />
                    <label htmlFor="agreeTerms" className="ml-2 text-sm">
                      {t("car_rental.agree_terms")}{" "}
                      <Link href="#" className="text-blue-600 hover:underline">
                        {t("car_rental.payment_terms")}
                      </Link>{" "}
                      {t("car_rental.of_green_wheel")}
                    </label>
                  </div>
                  {formik.touched.agreeTerms && formik.errors.agreeTerms && (
                    <div className="text-red-500 text-sm">{formik.errors.agreeTerms}</div>
                  )}

                  <div className="flex items-start">
                    <input
                      id="agreeDataPolicy"
                      name="agreeDataPolicy"
                      type="checkbox"
                      checked={formik.values.agreeDataPolicy}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                      className="mt-1"
                    />
                    <label htmlFor="agreeDataPolicy" className="ml-2 text-sm">
                      {t("car_rental.agree_data_policy")}{" "}
                      <Link href="#" className="text-blue-600 hover:underline">
                        {t("car_rental.data_sharing_terms")}
                      </Link>{" "}
                      {t("car_rental.of_green_wheel")}
                    </label>
                  </div>
                  {formik.touched.agreeDataPolicy && formik.errors.agreeDataPolicy && (
                    <div className="text-red-500 text-sm">
                      {formik.errors.agreeDataPolicy}
                    </div>
                  )}
                </div>
              </div>

              {/* Cột phải */}
              <div>
                <div className="bg-gray-50 p-4 rounded-lg">
                  <div className="flex items-center space-x-4">
                    <div className="relative overflow-hidden rounded-md w-40 h-28 sm:w-48 sm:h-32 md:w-56 md:h-36">
                      <ImageStyled
                        src="https://vinfastninhbinh.com.vn/wp-content/uploads/2024/06/vinfast-vf3-5.png"
                        alt={t("car_rental.vehicle")}
                        className="absolute inset-0 w-full h-full object-contain"
                        width={800}
                        height={520}
                      />
                    </div>
                    <div>
                      <h3 className="text-lg font-medium">{t("car_rental.vehicle")}</h3>
                    </div>
                  </div>

                  <div className="mt-4 bg-blue-50 p-4 rounded-lg">
                    <div className="flex justify-between items-center">
                      <h4 className="font-medium">Hà Nội</h4>
                      <button type="button" className="text-blue-600" />
                    </div>
                    <div className="mt-2 flex items-center">
                      <span>1 ngày • 16:50 24/09/2025 → 16:50 25/09/2025</span>
                    </div>
                    <div className="mt-1">
                      <span className="text-sm">Hình thức thuê: Theo ngày</span>
                    </div>
                  </div>

                  <div className="mt-4">
                    <h4 className="font-medium">{t("car_rental.detail_table")}</h4>
                    <div className="mt-2 space-y-2">
                      <div className="flex justify-between">
                        <span>{t("car_rental.listed_fee")}</span>
                        <span>{formatCurrency(LISTED_FEE)}</span>
                      </div>
                      <div className="border-t pt-2 flex justify-between font-medium">
                        <span>{t("car_rental.total")}</span>
                        <span>{formatCurrency(LISTED_FEE)}</span>
                      </div>
                      <div className="flex justify-between">
                        <span>{t("car_rental.deposit")}</span>
                        <span>{formatCurrency(DEPOSIT)}</span>
                      </div>
                    </div>
                  </div>

                  <div className="mt-6 border-t pt-4">
                    <div className="flex justify-between items-center">
                      <div>
                        <span className="font-medium">{t("car_rental.payment")}</span>
                        <span className="text-xs text-gray-500 block">
                          {t("car_rental.price_includes_vat")}
                        </span>
                      </div>
                      <span className="text-2xl font-bold text-green-500">
                        {formatCurrency(totalPayment)}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {/* Submit */}
            <div className="mt-8 text-center">
              <ButtonStyled
                type="submit"
                isDisabled={!formik.isValid || formik.isSubmitting}
                isLoading={formik.isSubmitting}
                color={!formik.isValid || formik.isSubmitting ? "default" : "success"}
                variant={!formik.isValid || formik.isSubmitting ? "flat" : "solid"}
                className="px-8 py-2 rounded-md"
              >
                {t("car_rental.pay")}
              </ButtonStyled>
            </div>
          </form>
        </div>
      ) : (
        <div className="text-center text-gray-600">Loading...</div>
      )}
    </div>
  )
}
