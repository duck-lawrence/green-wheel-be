import { DEFAULT_DATE_TIME_FORMAT, DEFAULT_TIMEZONE } from "@/constants/constants"
import dayjs from "dayjs"
import { DateValue, parseDateTime } from "@internationalized/date"

export const useDay = ({
    defaultFormat = DEFAULT_DATE_TIME_FORMAT
}: {
    defaultFormat?: string
}) => {
    const toCalenderDateTime = (
        dateTime: string | number | Date | dayjs.Dayjs | null | undefined
    ) => {
        if (!dateTime) return null
        const str = dayjs(dateTime).format(defaultFormat)
        return parseDateTime(str)
    }

    const formatDateTime = ({
        value,
        timeZone = DEFAULT_TIMEZONE
    }: {
        value: DateValue
        timeZone?: string
    }) => {
        if (!value) return ""
        const date = dayjs(value.toDate(timeZone))
        return date.format(defaultFormat)
    }

    return { toCalenderDateTime, formatDateTime }
}
