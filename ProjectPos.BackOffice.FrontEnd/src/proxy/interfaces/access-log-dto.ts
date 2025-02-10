import { EntityDto } from "../entity-dtos/entity-dto";
import { UserDto } from "./user-dto";

export interface AccessLogDto extends EntityDto<number> {
    userId?: number;
    userName?: string;
    logInTime?: Date;
    logOutTime?: Date;
    isLoggedIn?: boolean;
    computerName?: string;
    computerUserName?: string;
    computerIpAddress?: string;
    jwtToken?: string;
    user?: UserDto
}
