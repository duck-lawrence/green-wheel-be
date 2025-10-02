"use client"

import React, { useState, useEffect } from "react"
import { Formik, Form, Field, ErrorMessage } from "formik"
import * as Yup from "yup"
import { Icon } from "@iconify/react"
import { useTranslation } from "react-i18next"
import Link from "next/link"
import { useProfileStore, useToken } from "@/hooks"

export const RegisterReceiveForm = () => {
  const { t} = useTranslation("common")

  const [mounted, setMounted] = useState(false)

  const { user } = useProfileStore()
  const isLoggedIn = useToken((s) => !!s.accessToken)


  const RegisterCarSchema = Yup.object().shape({
    fullName: Yup.string().required(t("fullName.required")),
    phone: Yup.string()
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
    agreeDataPolicy: Yup.boolean().oneOf([true], t("agreeDataPolicy.required"))
  })

  useEffect(() => {
    setMounted(true)
  }, [])

  const initialValues = {
    fullName: isLoggedIn && user ? `${user.firstName} ${user.lastName}` : "",
    phone: isLoggedIn && user && user.phone ? user.phone : "",
    email: isLoggedIn && user ? user.email : "",
    isVingroup: false,
    pickupLocation: "",
    referralCode: "",
    note: "",
    paymentMethod: "",
    promotionCode: "",
    agreeTerms: false,
    agreeDataPolicy: false
  }

  const handleSubmit = (values: any) => {
    console.log("Form values:", values)
    // Xử lý gửi form
  }

  if (!mounted) return null

  const listedFee = 590000
  const deposit = 5000000
  const totalPayment = listedFee + deposit

  const formatCurrency = (amount: number) => {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "đ"
  }

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-5xl w-full max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center p-6">
          <h2 className="text-3xl font-bold">{t("car_rental.register_title")}</h2>
          <button type="button" className="text-gray-500 hover:text-gray-700">
            <Icon icon="ph:x" width={24} height={24} />
          </button>
        </div>

        <Formik
          initialValues={initialValues}
          validationSchema={RegisterCarSchema}
          onSubmit={handleSubmit}
        >
          {({ errors, touched }) => (
            <Form className="p-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                  {/* Thông tin người thuê */}
                  <div className="space-y-4">
                    <div>
                      <label htmlFor="fullName" className="block text-sm font-medium mb-1">
                        {t("car_rental.renter_name")}<span className="text-red-500">*</span>
                      </label>
                      <Field
                        type="text"
                        id="fullName"
                        name="fullName"
                        placeholder={t("car_rental.renter_name_placeholder")}
                        className={`w-full border rounded-md p-2 ${errors.fullName && touched.fullName ? "border-red-500" : "border-gray-300"}`}
                      />
                      <ErrorMessage name="fullName" component="div" className="text-red-500 text-sm mt-1" />
                    </div>

                    <div>
                      <label htmlFor="phone" className="block text-sm font-medium mb-1">
                        {t("car_rental.phone")}<span className="text-red-500">*</span>
                      </label>
                      <div className="relative">
                        <Field
                          type="text"
                          id="phone"
                          name="phone"
                          placeholder={t("car_rental.phone_placeholder")}
                          className={`w-full border rounded-md p-2 ${errors.phone && touched.phone ? "border-red-500" : "border-gray-300"}`}
                        />
                      </div>
                      <ErrorMessage name="phone" component="div" className="text-red-500 text-sm mt-1" />
                      <p className="text-xs text-red-500 mt-1">{t("car_rental.phone_min_length")}</p>
                      <p className="text-xs text-red-500">{t("car_rental.phone_verification")}</p>
                    </div>
                    
                    <div>
                      <label htmlFor="email" className="block text-sm font-medium mb-1">
                        {t("car_rental.email")}<span className="text-red-500">*</span>
                      </label>
                      <Field
                        type="email"
                        id="email"
                        name="email"
                        placeholder="wheel@fpt.edu.vn"
                        className={`w-full border rounded-md p-2 ${errors.email && touched.email ? "border-red-500" : "border-gray-300"}`}
                      />
                      <ErrorMessage name="email" component="div" className="text-red-500 text-sm mt-1" />
                    </div>

                    <div>
                      <label htmlFor="pickupLocation" className="block text-sm font-medium mb-1">
                        {t("car_rental.pickup_location")}<span className="text-red-500">*</span>
                      </label>
                      <div className="relative">
                        <Field
                          as="select"
                          id="pickupLocation"
                          name="pickupLocation"
                          className={`w-full border rounded-md p-2 appearance-none ${errors.pickupLocation && touched.pickupLocation ? "border-red-500" : "border-gray-300"}`}
                        >
                          <option value="">{t("car_rental.select_pickup_location")}</option>
                          <option value="place1">{t("car_rental.place1")}</option>
                          <option value="place2">{t("car_rental.place2")}</option>
                        </Field>
                        <div className="absolute right-2 top-1/2 -translate-y-1/2 pointer-events-none">
                          <Icon icon="ph:caret-down" />
                        </div>
                      </div>
                      <ErrorMessage name="pickupLocation" component="div" className="text-red-500 text-sm mt-1" />
                    </div>

                    <div>
                      <label htmlFor="note" className="block text-sm font-medium mb-1">
                        {t("car_rental.note")}
                      </label>
                      <Field
                        as="textarea"
                        id="note"
                        name="note"
                        className="w-full border rounded-md p-2 h-24 border-gray-300"
                      />
                    </div>
                  </div>

                  {/* Phương thức thanh toán */}
                  <div className="mt-6">
                    <h3 className="font-medium mb-3">{t("car_rental.payment_method")}</h3>
                    <Field
                      as="select"
                      id="paymentMethod"
                      name="paymentMethod"
                      className={`w-full border rounded-md p-2 appearance-none ${errors.paymentMethod && touched.paymentMethod ? "border-red-500" : "border-gray-300"}`}
                    >
                      <option value="">{t("car_rental.select_payment_method")}</option>
                      <option value="banking">{t("car_rental.bank_transfer")}</option>
                      <option value="momo">{t("car_rental.momo_wallet")}</option>
                    </Field>
                    <ErrorMessage name="paymentMethod" component="div" className="text-red-500 text-sm mt-1" />
                  </div>

                  {/* Điều khoản */}
                  <div className="mt-6 space-y-3">
                    <div className="flex items-start">
                      <Field
                        type="checkbox"
                        id="agreeTerms"
                        name="agreeTerms"
                        className="mt-1"
                      />
                      <label htmlFor="agreeTerms" className="ml-2 text-sm">
                        {t("car_rental.agree_terms")} <Link href="#" className="text-blue-600 hover:underline">{t("car_rental.payment_terms")}</Link> {t("car_rental.of_green_wheel")}
                      </label>
                    </div>
                    <ErrorMessage name="agreeTerms" component="div" className="text-red-500 text-sm" />

                    <div className="flex items-start">
                      <Field
                        type="checkbox"
                        id="agreeDataPolicy"
                        name="agreeDataPolicy"
                        className="mt-1"
                      />
                      <label htmlFor="agreeDataPolicy" className="ml-2 text-sm">
                        {t("car_rental.agree_data_policy")} <Link href="#" className="text-blue-600 hover:underline">{t("car_rental.data_sharing_terms")}</Link> {t("car_rental.of_green_wheel")}
                      </label>
                    </div>
                    <ErrorMessage name="agreeDataPolicy" component="div" className="text-red-500 text-sm" />
                  </div>
                </div>

                {/* Thông tin xe */}
                <div>
                  <div className="bg-gray-50 p-4 rounded-lg">
                    <div className="flex items-center space-x-4">
                      <div className="w-32 h-24 relative">
                        <img 
                          src="https://vinfastninhbinh.com.vn/wp-content/uploads/2024/06/vinfast-vf3-5.png" 
                          alt={t("car_rental.vehicle")}
                          className="rounded-md w-full h-full object-cover"
                          
                        />
                      </div>
                      <div>
                        <h3 className="text-lg font-medium">{t("car_rental.vehicle")}</h3>
                      </div>
                    </div>

                     <div className="mt-4 bg-blue-50 p-4 rounded-lg">
                      <div className="flex justify-between items-center">
                        <h4 className="font-medium">Hà Nội</h4>
                        <button type="button" className="text-blue-600">
                       
                        </button>
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
                          <span className="text-xs text-gray-500 block">{t("car_rental.price_includes_vat")}</span>
                        </div>
                        <span className="text-2xl font-bold text-green-500">{formatCurrency(totalPayment)}</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div className="mt-8 text-center">
                <button 
                  type="submit" 
                  className="bg-green-600 hover:bg-green-700 text-white px-8 py-2 rounded-md"
                >
                  {t("car_rental.pay")} {formatCurrency(totalPayment)}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  )
}

