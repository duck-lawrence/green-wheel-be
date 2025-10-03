"use client"

import React, { useState, useEffect } from "react"
import { useFormik } from "formik"
import * as Yup from "yup"
import { useTranslation } from "react-i18next"
import Link from "next/link"
import { useProfileStore, useToken } from "@/hooks"
import { ButtonStyled, InputStyled, LocalFilter , EnumPicker,ImageStyled } from "@/components"
import { PaymentMethod } from "@/constants/enum"
import { PaymentMethodLabels } from "@/constants/labels"

type FormValues = {
  fullName: string
  phone: string
  email: string
  pickupLocation: string
  referralCode: string
  note: string
  paymentMethod: PaymentMethod | null 
  promotionCode: string
  agreeTerms: boolean
  agreeDataPolicy: boolean
}

export const RegisterReceiveForm = () => {
  const { t } = useTranslation("common")

  const [mounted, setMounted] = useState(false)
  const { user } = useProfileStore()
  const isLoggedIn = useToken((s) => !!s.accessToken)


  useEffect(() => {
    setMounted(true)
  }, [])
// Chỉnh lấy api 
  const listedFee = 590000
  const deposit = 5000000
  const totalPayment = listedFee + deposit
  const formatCurrency = (n: number) => new Intl.NumberFormat("vi-VN").format(n) + "đ"

  const initialValues: FormValues = {
    fullName: isLoggedIn && user ? `${user.firstName} ${user.lastName}` : "",
    phone: isLoggedIn && user && user.phone ? user.phone : "",
    email: isLoggedIn && user ? user.email : "",
    pickupLocation: "",
    referralCode: "",
    note: "",
    paymentMethod: null,
    promotionCode: "",
    agreeTerms: false,
    agreeDataPolicy: false,
  }

  const formik = useFormik<FormValues>({
    enableReinitialize: true,
    validateOnMount: true,
    initialValues,
    validationSchema: Yup.object().shape({
      fullName: Yup.string().required(t("fullName.required")),
      phone: Yup.string()
        .min(9, t("car_rental.phone_min_length"))
        .matches(/^[0-9]{9,11}$/, t("phone.invalid"))
        .required(t("phone.required")),
      email: Yup.string().email(t("email.invalid")).required(t("email.required")),
      isVingroup: Yup.boolean(),
      pickupLocation: Yup.string().required(t("pickupLocation.required")),
      referralCode: Yup.string(),
      note: Yup.string(),
      paymentMethod: Yup.string().required(t("paymentMethod.required")),
      promotionCode: Yup.string(),
      agreeTerms: Yup.boolean().oneOf([true], t("agreeTerms.required")),
      agreeDataPolicy: Yup.boolean().oneOf([true], t("agreeDataPolicy.required")),
    }),
    onSubmit: (values) => {
      console.log("Form values:", values)
      // TODO: call API submit
    },
  })
  const renterFilled = !!formik.values.fullName?.trim()
  const emailFilled  = !!formik.values.email?.trim()
  return (
    <div className="min-h-screen bg-gray-50 py-10 px-4 sm:px-6 lg:px-8 mt-30">
      {mounted ? (
        <div className="mx-auto max-w-5xl bg-white rounded-lg ">
          <div className="flex items-center justify-between border-b border-gray-100 px-6 py-6">
            <h2 className="text-3xl font-bold">{t("car_rental.register_title")}</h2>
          </div>

          <form onSubmit={formik.handleSubmit} className="px-6 pb-8 pt-6" noValidate>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Cột trái: tất cả input */}
              <div>
                <div className="space-y-4">
                  {/* Renter name */}
                  <InputStyled
                    variant="bordered"
                    label={t("car_rental.renter_name")}
                    placeholder={t("car_rental.renter_name_placeholder")}
                    value={formik.values.fullName}
                    onValueChange={(v) => formik.setFieldValue("fullName", v)}
                    isInvalid={!!(formik.touched.fullName && formik.errors.fullName)}
                    errorMessage={formik.touched.fullName ? formik.errors.fullName : undefined}
                    onBlur={() => formik.setFieldTouched("fullName", true)}
                    isClearable={false}
                    readOnly={isLoggedIn}
                    classNames={{
                      inputWrapper: renterFilled ? "bg-gray-100 border-gray-200" : undefined,
                      input: renterFilled ? "text-gray-600 placeholder:text-gray-400" : undefined,
                      label: renterFilled ? "text-gray-500" : undefined,
                          }}  
                  />

                  {/* Phone number */}
                  <InputStyled
                    variant="bordered"
                    label={t("car_rental.phone")}
                    placeholder={t("car_rental.phone_placeholder")}
                    type="tel"
                    inputMode="numeric"
                    value={formik.values.phone}
                    onValueChange={(v) => formik.setFieldValue("phone", v)}
                    isInvalid={!!(formik.touched.phone && formik.errors.phone)}
                    errorMessage={formik.touched.phone ? formik.errors.phone : undefined}
                    onBlur={() => formik.setFieldTouched("phone", true)}
                    onClear={() => formik.setFieldValue("phone", "")}

                    
                  />
                 

                  {/* Email */}
                  <InputStyled
                    variant="bordered"
                    label={t("car_rental.email")}
                    placeholder="wheel@fpt.edu.vn"
                    type="email"
                    value={formik.values.email}
                    onValueChange={(v) => formik.setFieldValue("email", v)}
                    isInvalid={!!(formik.touched.email && formik.errors.email)}
                    errorMessage={formik.touched.email ? formik.errors.email : undefined}
                    onBlur={() => formik.setFieldTouched("email", true)}
                    isClearable={false}
                    readOnly={isLoggedIn}
                    classNames={{
                      inputWrapper: emailFilled ? "bg-gray-100 border-gray-200" : undefined,
                      input: emailFilled ? "text-gray-600 placeholder:text-gray-400" : undefined,
                      label: emailFilled ? "text-gray-500" : undefined,
                    }}
                  />

                  {/* Pickup location (input) */}
                  <LocalFilter
                    value={formik.values.pickupLocation || null}
                    onChange={(val) => {
                      formik.setFieldValue("pickupLocation", val ?? "")
                      formik.setFieldTouched("pickupLocation", true)
                    }}
                  />
                  {formik.touched.pickupLocation && formik.errors.pickupLocation && (
                    <p className="text-red-500 text-sm mt-1">{formik.errors.pickupLocation}</p>
                  )}
                  {/* <LocalFilter
                    label={
                      <>
                        {t("car_rental.pickup_location")}
                        <span className="text-danger-500 ml-1">*</span>
                      </>
                    }
                    isRequired
                    placeholder={t("car_rental.select_pickup_location")}
                    options={[
                      { value: "place1", label: t("car_rental.place1") },
                      { value: "place2", label: t("car_rental.place2") },
                    ]}
                    value={ // tìm object trong options có value trùng với formik.values.pickupLocation nhưng mà đang là string
                      [
                        { value: "place1", label: t("car_rental.place1") },
                        { value: "place2", label: t("car_rental.place2") },
                      ].find(o => o.value === formik.values.pickupLocation) ?? null
                    }
                    onValueChange={(opt: { value: string; label: string } | null) =>
                      formik.setFieldValue("pickupLocation", opt?.value ?? "")
                    }
                    onBlur={() => formik.setFieldTouched("pickupLocation", true)}
                    isInvalid={Boolean(formik.touched.pickupLocation && formik.errors.pickupLocation)}
                    errorMessage={
                      formik.touched.pickupLocation && formik.errors.pickupLocation
                        ? formik.errors.pickupLocation
                        : undefined
                    }
                /> */}
                  {/* Note (input) */}
                  <InputStyled
                    variant="bordered"
                    label={t("car_rental.note")}
                    placeholder=""
                    value={formik.values.note}
                    onValueChange={(v) => formik.setFieldValue("note", v)}
                    isInvalid={!!(formik.touched.note && formik.errors.note)}
                    errorMessage={formik.touched.note ? formik.errors.note : undefined}
                    onBlur={() => formik.setFieldTouched("note", true)}
                    onClear={() => formik.setFieldValue("note", "")}
                  />

                  {/* Payment method (input) */}
                  {/* <div className="mt-6">
                <h3 className="font-medium mb-3">{t("car_rental.payment_method")}</h3>
                <select
                  id="paymentMethod"
                  name="paymentMethod"
                  value={formik.values.paymentMethod}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  className={`w-full border rounded-md p-2 appearance-none ${
                    formik.errors.paymentMethod && formik.touched.paymentMethod
                      ? "border-red-500"
                      : "border-gray-300"
                  }`}
                >
                  <option value="">{t("car_rental.select_payment_method")}</option>
                  <option value="banking">{t("car_rental.bank_transfer")}</option>
                  <option value="momo">{t("car_rental.momo_wallet")}</option>
                </select>
                {formik.touched.paymentMethod && formik.errors.paymentMethod && (
                  <div className="text-red-500 text-sm mt-1">{formik.errors.paymentMethod}</div>
                )}
              </div>    */}     
                
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
                      type="checkbox"
                      id="agreeTerms"
                      name="agreeTerms"
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
                      type="checkbox"
                      id="agreeDataPolicy"
                      name="agreeDataPolicy"
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

              {/* Cột phải: Thông tin xe */}
              <div>
                <div className="bg-gray-50 p-4 rounded-lg">
                  <div className="flex items-center space-x-4">
                    <div className="
                      relative overflow-hidden rounded-md
                      w-40 h-28
                      sm:w-48 sm:h-32
                      md:w-56 md:h-36
                    ">
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
                        <span>{formatCurrency(listedFee)}</span>
                      </div>
                      <div className="border-t pt-2 flex justify-between font-medium">
                        <span>{t("car_rental.total")}</span>
                        <span>{formatCurrency(listedFee)}</span>
                      </div>
                      <div className="flex justify-between">
                        <span>{t("car_rental.deposit")}</span>
                        <span>{formatCurrency(deposit)}</span>
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
              // tuỳ chọn: đổi màu/variant theo trạng thái cho giống bg-green-600
              color={!formik.isValid || formik.isSubmitting ? "default" : "success"}
              variant={!formik.isValid || formik.isSubmitting ? "flat" : "solid"}
              className="px-8 py-2 rounded-md"
            >
              {/* {t("car_rental.pay")} {formatCurrency(totalPayment)} */}
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
