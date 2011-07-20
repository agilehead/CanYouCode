using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using canyoucode.Core;

namespace canyoucode.Web.I18n
{
    public class MessageTexts : Dictionary<string, string>
    {
        public MessageTexts()
        {
            //Maintain this list alphabetically sorted.
            this[MessageCodes.ACCOUNT_CREATED_FOR_MARKETING] = "Account created for marketing. Activation Link - {0}, Emailid - {1}";
            this[MessageCodes.ACCOUNT_NOT_ACTIVE] = "Account is not active.";
            this[MessageCodes.ACCOUNT_ACTIVATED] = "You account is now active.";
            this[MessageCodes.ACCOUNT_REMOVED] = "Account Removed.";
            this[MessageCodes.ACCOUNT_DEACTIVATED] = "Account has been deactivated.";
            this[MessageCodes.ACTIVATE_ACCOUNT_TO_BID] = "Activate your account to place bids.";
            this[MessageCodes.BID_PLACED] = "Bid has been placed.";
            this[MessageCodes.BID_FAILED] = "Bid failed.";
            this[MessageCodes.BID_WITHDRAWN] = "The Bid has been withdrawn";
            this[MessageCodes.CONSULTANT_ADDED] = "Person added to your company.";
            this[MessageCodes.CONSULTANT_REMOVED] = "Consultant Removed.";
            this[MessageCodes.CONSULTANT_ADD_FAILED] = "Error adding data. Please try again.";
            this[MessageCodes.CONSULTANT_SAVED] = "Consultant Saved.";
            this[MessageCodes.DECLINED_INVITE] = "The invite has been declined.";
            this[MessageCodes.DECLINE_INVITE_FAILED] = "Decline Invite Failed.";
            this[MessageCodes.INVALID_EMAIL] = "Invalid Email.";
            this[MessageCodes.INVALID_USERNAME] = "Invalid Username.";
            this[MessageCodes.INVITES_SENT] = "The invites have been sent.";
            this[MessageCodes.OLD_PASSWORD_MISMATCH] = "Current password provided is not valid.";
            this[MessageCodes.PASSWORD_MISMATCH] = "The username or password you have entered is incorrect.";
            this[MessageCodes.PASSWORD_CHANGED] = "Password changed.";
            this[MessageCodes.PORTFOLIO_ENTRY_SAVED] = "Portfolio entry saved.";
            this[MessageCodes.PORTFOLIO_ENTRY_REMOVED] = "Portfolio entry removed.";
            this[MessageCodes.PORTFOLIO_ENTRY_SAVE_FAILED] = "Portfolio entry save failed.";
            this[MessageCodes.PROFILE_UPDATED] = "Settings saved.";
            this[MessageCodes.RESET_LINK_SENT] = "A link to reset your password has been set to your email address.";
            this[MessageCodes.RESET_LINK_SEND_FAILED] = "Reset Link send failed. Try again.";
            this[MessageCodes.SUMMARY_SAVED] = "Summary saved.";
            this[MessageCodes.SUMMARY_SAVE_FAILED] = "Summary save failed.";
            this[MessageCodes.SENT] = "Message Sent.";
            this[MessageCodes.SEND_FAILED] = "Message Send Failed.";
            this[MessageCodes.WITHDRAW_BID_FAILED] = "Withdraw Bid failed.";
        }
    }
}